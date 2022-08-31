using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project2.WebAPI.Dtos
{
	/// <summary>
	///   <br />
	/// </summary>
	public class DtoUserAuthenticationRequest
	{
		/// <summary>Gets or sets the username.</summary>
		/// <value>The username.</value>
		[Required(ErrorMessage = "The user name is required")]
		public string Username { get; set; }

		/// <summary>Gets or sets the password.</summary>
		/// <value>The password.</value>
		[PasswordPropertyText]
		[Required(ErrorMessage = "The password is required")]
		public string Password { get; set; }
	}
}