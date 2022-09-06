using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project2.WebAPI.DAL.Entities;
using Project2.WebAPI.Utils;
using Project2.WebAPI.Utils.Dtos;
using Project2.WebAPI.Utils.Exceptions;
using Project2.WebAPI.Utils.Session;

namespace Project2.WebAPI.DAL.Services.Security
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="DataServiceBase" />
	/// <seealso cref="ISecurityService" />
	public class SecurityService : DataServiceBase, ISecurityService
	{
		private static readonly MyWebApiException FailedLogin = new MyWebApiException(HttpStatusCode.Unauthorized,
			"Incorrect password and/ password. Please correct and try again");

		private readonly SignInManager<EntitySystemUser> _signInManager;
	//	private readonly UserManager<EntitySystemUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityService" /> class.
		/// </summary>
		/// <param name="webSettings">The web settings.</param>
		/// <param name="officeDbContext">The office database context.</param>
		/// <param name="session">The session.</param>
		/// <param name="signInManager">The sign in manager.</param>
		/// <param name="roleManager">The role manager.</param>
		public SecurityService(
			IWebApiSettings webSettings,
			ConnectedOfficeDbContext officeDbContext,
			IUserSession session,
			SignInManager<EntitySystemUser> signInManager,
			RoleManager<IdentityRole> roleManager) : base(webSettings, officeDbContext, session)
		{
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		/// <summary>
		/// Logins the user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>
		/// DtoUserAuthenticationResponse
		/// </returns>
		public async ValueTask<DtoUserAuthenticationResponse> LoginUserAsync(DtoUserAuthenticationRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.UserName))
				throw new MyWebApiException(HttpStatusCode.BadRequest,
					"Please specify a valid username");

			if (string.IsNullOrWhiteSpace(request.Password))
				throw new MyWebApiException(HttpStatusCode.BadRequest,
					"Please specify a valid password");

			var systemUser = await _signInManager.UserManager.FindByNameAsync(request.UserName);
			if (systemUser == null)
				throw FailedLogin;

			var result = await _signInManager.PasswordSignInAsync(systemUser, request.Password, isPersistent: false, lockoutOnFailure: false);

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
			await OfficeDbContext.UserSession.AddAsync(userSession);
			await OfficeDbContext.SaveChangesAsync();

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
				issuer: WebSettings.JwtIssuer,
				audience: WebSettings.JwtAudience,
				expires: DateTime.Now.AddHours(5), // expires after 5 hours
				claims: claimList,
				signingCredentials: new SigningCredentials(
					key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebSettings.JwtSecret)),
					algorithm: SecurityAlgorithms.HmacSha256)
			);

			await _signInManager.SignInAsync(systemUser, false);
			return systemUser.ToDtoSystemUser(roleName, token);
		}


		/// <summary>
		/// Log outs the user asynchronous.
		/// </summary>
		/// <returns></returns>
		public async ValueTask LogoutUserAsync()
		{
			var currentSession = await OfficeDbContext.UserSession.AsTracking().FirstOrDefaultAsync(e => e.SessionId == Session.SessionToken);

			if (currentSession != null)
			{
				currentSession.LogoutDate = DateTime.UtcNow;
				OfficeDbContext.Entry(currentSession).State = EntityState.Modified;
				await OfficeDbContext.SaveChangesAsync();
			}

			await _signInManager.SignOutAsync();
		}

		/// <summary>
		/// Determines whether [is user session valid asynchronous].
		/// </summary>
		/// <returns>
		///   <c>true</c> if [is user session valid asynchronous]; otherwise, <c>false</c>.
		/// </returns>
		public async ValueTask<bool> IsUserSessionValidAsync()
		{
			var currentSession = await OfficeDbContext.UserSession
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.SessionId == Session.SessionToken);

			return (currentSession != null && currentSession.LogoutDate == null);
		}

		/// <summary>
		/// Registers the admin user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public async ValueTask<DtoUserRegistrationResponse> RegisterAdminUserAsync(DtoUserRegistrationRequest request)
		{
			return await CreateUserAsync(request, ApiConstants.UserRoles.Admin);
		}

		/// <summary>
		/// Registers the normal user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public async ValueTask<DtoUserRegistrationResponse> RegisterNormalUserAsync(DtoUserRegistrationRequest request)
		{
			return await CreateUserAsync(request, ApiConstants.UserRoles.User);
		}

		/// <summary>
		/// Gets the system user by identifier asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public async ValueTask<DtoSystemUser> GetSystemUserByIdAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						$"The user-id specified is not valid (id = '{id}')");

				var systemUser = await _signInManager.UserManager.FindByIdAsync(id.ToString("D"));
				if (systemUser == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No system user with id = '{id}' has been found");

				var rolesList = await _signInManager.UserManager.GetRolesAsync(systemUser);
				var roleName = rolesList.FirstOrDefault() ?? ApiConstants.UserRoles.User;

				return systemUser.ToDtoSystemUser(roleName);

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

		#region Privates

		/// <summary>
		/// Creates the user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="roleName">Name of the role.</param>
		/// <returns></returns>
		/// <exception cref="Utils.Exceptions.MyWebApiException">
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
		/// <exception cref="MyWebApiException">The Username specified is not valid. Please correct and try again
		/// or
		/// A system user already exists with username = '{request.UserName}'
		/// or</exception>
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
						
					if(result.Errors != null && result.Errors.Any())
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
				if(!await _roleManager.RoleExistsAsync(roleName))
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
