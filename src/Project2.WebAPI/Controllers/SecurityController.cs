using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project2.WebAPI.DAL;
using Project2.WebAPI.DAL.Converters;
using Project2.WebAPI.DAL.Dtos;
using Project2.WebAPI.DAL.Entities;
using Project2.WebAPI.Utils;
using Project2.WebAPI.Utils.Exceptions;
using Project2.WebAPI.Utils.Session;

namespace Project2.WebAPI.Controllers
{
	/// <summary>
	/// The api/security controller
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[Route("api/security")]
	[Authorize(Roles = ApiConstants.UserRoles.AdminOrUser)]
	[ApiController]
	public class SecurityController : ControllerBase
	{
		private static readonly MyWebApiException FailedLogin = new MyWebApiException(HttpStatusCode.Unauthorized,
			"Incorrect password and/ password. Please correct and try again");

		private readonly IWebApiSettings _webSettings;
		private readonly ConnectedOfficeDbContext _officeDbContext;
		private readonly IUserSession _userSession;
		private readonly SignInManager<EntitySystemUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;


		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityController" /> class.
		/// </summary>
		/// <param name="webSettings">The web settings.</param>
		/// <param name="officeDbContext">The office database context.</param>
		/// <param name="userSession">The user session.</param>
		/// <param name="signInManager">The sign in manager.</param>
		/// <param name="roleManager">The role manager.</param>
		public SecurityController(
			IWebApiSettings webSettings,
			ConnectedOfficeDbContext officeDbContext
		, IUserSession userSession,
			SignInManager<EntitySystemUser> signInManager,
			RoleManager<IdentityRole> roleManager)
		{
			_webSettings = webSettings;
			_officeDbContext = officeDbContext;
			_userSession = userSession;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		/// <summary>
		/// logs in a system user - admin or normal user.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns>
		/// DtoUserAuthenticationResponse
		/// </returns>
		[HttpGet("login/{username}/{password}")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(DtoUserAuthenticationResponse), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoUserAuthenticationResponse>> LoginUserAsync(string username, string password)
		{
			if (string.IsNullOrWhiteSpace(username))
				throw new MyWebApiException(HttpStatusCode.BadRequest,
					"Please specify a valid username");

			if (string.IsNullOrWhiteSpace(password))
				throw new MyWebApiException(HttpStatusCode.BadRequest,
					"Please specify a valid password");

			try
			{
				var systemUser = await _signInManager.UserManager.FindByNameAsync(username);
				if (systemUser == null)
					throw FailedLogin;

				var result = await _signInManager.PasswordSignInAsync(systemUser, password, isPersistent: false, lockoutOnFailure: false);

				if (!result.Succeeded)
					throw FailedLogin;

				var rolesList = await _signInManager.UserManager.GetRolesAsync(systemUser);
				var roleName = rolesList.FirstOrDefault() ?? ApiConstants.UserRoles.User;

				// create user session
				var userSession = new EntityUserSession
				{
					SessionId = Guid.NewGuid(),
					DateCreated = DateTime.UtcNow,
					LogoutDate = null,
				};
				await _officeDbContext.UserSession.AddAsync(userSession);
				await _officeDbContext.SaveChangesAsync();

				var claimList = new List<Claim>()
				{
					new Claim(ApiConstants.UserClaims.UserName, systemUser.UserName),
					new Claim(ApiConstants.UserClaims.EmailAddress, systemUser.Email),
					new Claim(ApiConstants.UserClaims.PhoneNumber, systemUser.PhoneNumber),
					new Claim(ApiConstants.UserClaims.GivenName, systemUser.UserName),
					new Claim(ApiConstants.UserClaims.Role, roleName),
					new Claim(ApiConstants.UserClaims.SessionToken, userSession.SessionId.ToString()),
				};


				var token = new JwtSecurityToken(
					issuer: _webSettings.JwtIssuer,
					audience: _webSettings.JwtAudience,
					expires: DateTime.Now.AddHours(3), // expires after 3 hours
					claims: claimList,
					signingCredentials: new SigningCredentials(
						key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_webSettings.JwtSecret)),
						algorithm: SecurityAlgorithms.HmacSha256)
				);

				await _signInManager.SignInAsync(systemUser, false);

				var response =  systemUser.ToDtoSystemUser(roleName, token);
				return Ok(response);
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}



		/// <summary>
		/// logs out a system user.
		/// </summary>
		/// <returns></returns>
		[HttpDelete("logout")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async ValueTask<ActionResult> LogoutUserAsync()
		{
			try
			{
				var currentSession = await _officeDbContext.UserSession.AsTracking().FirstOrDefaultAsync(e => e.SessionId == _userSession.SessionToken);

				if (currentSession != null)
				{
					currentSession.LogoutDate = DateTime.UtcNow;
					_officeDbContext.Entry(currentSession).State = EntityState.Modified;
					await _officeDbContext.SaveChangesAsync();
				}

				await _signInManager.SignOutAsync();
				return Ok("User has been successfully logged out");
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		/// <summary>
		/// registers an admin system user.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpPost("register/admin")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(DtoUserRegistrationResponse), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoUserRegistrationResponse>> RegisterAdminUserAsync([FromBody] DtoUserRegistrationRequest request)
		{
			try
			{
				var response = await CreateUserAsync(request, ApiConstants.UserRoles.Admin);

				return Created(new Uri(Url.Link("GetUser", new { id = response.User.Id })), response);
			}
			catch (MyWebApiException ex)
			{
					return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// registers a normal system user.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>DtoUserRegistrationResponse</returns>
		[HttpPost("register/user")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(DtoUserRegistrationResponse), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoUserRegistrationResponse>> RegisterNormalUserAsync([FromBody] DtoUserRegistrationRequest request)
		{
			try
			{
				var response = await CreateUserAsync(request, ApiConstants.UserRoles.User);

				return Created(new Uri(Url.Link("GetUser", new { id = response.User.Id })), response);
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		/// <summary>
		/// gets a particular system user by its id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get/{id}", Name = "GetUser")]
		[ProducesResponseType(typeof(DtoSystemUser), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoSystemUser>> GetSystemUserByIdAsync(Guid id)
		{
			if (id == Guid.Empty)
			{
				throw new MyWebApiException(HttpStatusCode.BadRequest,
					$"The user-id specified is not valid (id = '{id}')");
			}

			try
			{
				var systemUser = await _signInManager.UserManager.FindByIdAsync(id.ToString("D"));
				if (systemUser == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No system user with id = '{id}' has been found");

				var rolesList = await _signInManager.UserManager.GetRolesAsync(systemUser);
				var roleName = rolesList.FirstOrDefault() ?? ApiConstants.UserRoles.User;

				var response = systemUser.ToDtoSystemUser(roleName);

				return Ok(response);
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		#region Privates
		/// <summary>
		/// Creates the user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="roleName">Name of the role.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">
		/// Please specify a valid username
		/// or
		/// Please specify a valid password
		/// or
		/// Please specify a valid email address
		/// or
		/// Please specify a valid phone number
		/// or
		/// A system user already exists with username = '{request.UserName}'
		/// or
		/// </exception>
		private async ValueTask<DtoUserRegistrationResponse> CreateUserAsync(DtoUserRegistrationRequest request, string roleName)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(request.UserName))
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						"Please specify a valid username");

				if (string.IsNullOrWhiteSpace(request.Password))
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						"Please specify a valid password");

				if (string.IsNullOrWhiteSpace(request.EmailAddress))
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						"Please specify a valid email address");

				if (string.IsNullOrWhiteSpace(request.PhoneNumber))
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						"Please specify a valid phone number");

				var currentUser = await _signInManager.UserManager.FindByNameAsync(request.UserName);
				if (currentUser != null)
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						$"A system user already exists with username = '{request.UserName}'");

				var systemUser = request.ToEntitySystemUser();

				var result = await _signInManager.UserManager.CreateAsync(systemUser, request.Password);

				if (!result.Succeeded)
				{
					var errorMessage = "An unknown error occurred. Please correct and try again";

					if (result.Errors != null && result.Errors.Any())
					{
						var counter = 1;
						var sb = new StringBuilder();
						foreach (var error in result.Errors)
						{
							sb.AppendLine($"{counter}. {error.Code}-{error.Description}");
							counter++;
						}

						errorMessage = sb.ToString();
					}


					throw new MyWebApiException(HttpStatusCode.BadRequest, errorMessage);
				}

				// check if role-name exist, add if not
				if (!await _roleManager.RoleExistsAsync(roleName))
				{
					await _roleManager.CreateAsync(new IdentityRole(roleName));
				}

				// add the user to this role
				await _signInManager.UserManager.AddToRoleAsync(systemUser, roleName);

				return new DtoUserRegistrationResponse
				{
					Message = "System user created successfully",
					User = systemUser.ToDtoSystemUser(roleName)
				};
			}
			catch (MyWebApiException ex)
			{
				Console.WriteLine(ex);
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		#endregion

	}
}
