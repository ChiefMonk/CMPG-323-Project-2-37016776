﻿using System.Threading.Tasks;
using Project2.WebAPI.Dtos;

namespace Project2.WebAPI.Services.Security
{
	/// <summary>
	/// 
	/// </summary>
	public interface ISecurityService
	{
		/// <summary>
		/// Logins the user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>DtoUserAuthenticationResponse</returns>
		ValueTask<DtoUserAuthenticationResponse> LoginUserAsync(DtoUserAuthenticationRequest request);

		/// <summary>
		/// Determines whether [is user session valid asynchronous].
		/// </summary>
		/// <returns></returns>
		ValueTask<bool> IsUserSessionValidAsync();

		/// <summary>
		/// Logouts the user asynchronous.
		/// </summary>
		/// <returns></returns>
		ValueTask LogoutUserAsync();

		/// <summary>
		/// Registers the admin user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		ValueTask<DtoUserRegistrationResponse> RegisterAdminUserAsync(DtoUserRegistrationRequest request);

		/// <summary>
		/// Registers the normal user asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		ValueTask<DtoUserRegistrationResponse> RegisterNormalUserAsync(DtoUserRegistrationRequest request);
	}
}
