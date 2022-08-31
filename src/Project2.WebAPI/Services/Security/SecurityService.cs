using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project2.Data;
using Project2.Data.Entities;
using Project2.WebAPI.Aut;
using Project2.WebAPI.Dtos;
using Project2.WebAPI.Exceptions;
using Project2.WebAPI.Services.Category;
using Project2.WebAPI.Session;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Project2.WebAPI.Services.Security
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Project2.WebAPI.Services.Category.DataServiceBase" />
	/// <seealso cref="Project2.WebAPI.Services.Security.ISecurityService" />
	public class SecurityService : DataServiceBase, ISecurityService
	{
		private static readonly MyWebApiException FailedLogin = new MyWebApiException(HttpStatusCode.Unauthorized,
			"Incorrect password and/ password. Please correct and try again");

		private readonly SignInManager<EntitySystemUser> _signInManager;
		private readonly UserManager<EntitySystemUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityService" /> class.
		/// </summary>
		/// <param name="webApiSettings">The web API settings.</param>
		/// <param name="dbContext">The database context.</param>
		/// <param name="session">The session.</param>
		/// <param name="signInManager">The sign in manager.</param>
		/// <param name="userManager">The user manager.</param>
		/// <param name="roleManager">The role manager.</param>
		public SecurityService(
			IWebApiSettings webApiSettings,
			ConnectedOfficeDbContext dbContext,
			IUserSession session,
			SignInManager<EntitySystemUser> signInManager,
			UserManager<EntitySystemUser> userManager,
			RoleManager<IdentityRole> roleManager) : base(webApiSettings, dbContext, session)
		{
			_signInManager = signInManager;
			_userManager = userManager;
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
			var systemUser = await _userManager.FindByNameAsync(request.Username);
			if (systemUser == null)
				throw FailedLogin;

		//	var isValidUser = await _userManager.CheckPasswordAsync(systemUser, request.Password);
		//	if (!isValidUser)
			//	throw FailedLogin;

			var result = await _signInManager.PasswordSignInAsync(systemUser, request.Password, isPersistent: false, lockoutOnFailure: false);

			if (!result.Succeeded)
				throw FailedLogin;


			var claimList = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, systemUser.UserName),
				new Claim(ClaimTypes.Email, systemUser.Email),
				new Claim(ClaimTypes.MobilePhone, systemUser.PhoneNumber),
				new Claim(JwtRegisteredClaimNames.UniqueName, systemUser.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};

			var rolesList = await _userManager.GetRolesAsync(systemUser);

			foreach (var role in rolesList)
			{
				claimList.Add(new Claim(ClaimTypes.Role, role));
			}

			var token = new JwtSecurityToken(
				issuer: _webApiSettings.JwtIssuer,
				audience: _webApiSettings.JwtAudience,
				expires: DateTime.Now.AddHours(5), // expires after 5 hours
				claims: claimList,
				signingCredentials: new SigningCredentials(
					key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_webApiSettings.JwtSecret)),
					algorithm: SecurityAlgorithms.HmacSha256)
			);

			await _signInManager.SignInAsync(systemUser, false);
			return systemUser.ToDtoSystemUser(rolesList.FirstOrDefault(), token);
		}


		/// <summary>
		/// Logs out the user asynchronous.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <returns></returns>
		public async ValueTask LogoutUserAsync(string userName)
		{
			await _signInManager.UserManager.FindByNameAsync(userName);
			await _signInManager.SignOutAsync();
		}

		/// <summary>
		/// Registers the admin user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public async ValueTask<DtoUserRegistrationResponse> RegisterAdminUserAsync(DtoUserRegistrationRequest request)
		{
			return await CreateUserAsync(request, UserRoles.Admin);
		}

		/// <summary>
		/// Registers the normal user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public async ValueTask<DtoUserRegistrationResponse> RegisterNormalUserAsync(DtoUserRegistrationRequest request)
		{
			return await CreateUserAsync(request, UserRoles.User);
		}

		#region Privates

		/// <summary>
		/// Creates the user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="roleName">Name of the role.</param>
		/// <returns></returns>
		/// <exception cref="MyWebApiException">
		/// The Username specified is not valid. Please correct and try again
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
						$"The Username specified is not valid. Please correct and try again");

				var currentUser = await _userManager.FindByNameAsync(request.UserName);
				if (currentUser != null)
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						$"A system user already exists with username = '{request.UserName}'");

				var systemUser = request.ToEntitySystemUser();

				var result = await _userManager.CreateAsync(systemUser, request.Password);

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
				await _userManager.AddToRoleAsync(systemUser, roleName);

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
