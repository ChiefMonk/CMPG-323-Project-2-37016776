using System.Threading.Tasks;
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
		/// Logouts the user asynchronous.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <returns></returns>
		ValueTask LogoutUserAsync(string userName);

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
