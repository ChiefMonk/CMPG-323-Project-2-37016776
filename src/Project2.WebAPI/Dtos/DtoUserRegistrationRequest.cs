using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project2.WebAPI.Dtos
{
	/// <summary>
	///   <br />
	/// </summary>
	public class DtoUserRegistrationRequest
	{

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		[Required(ErrorMessage = "The user name is required")]
		public string UserName { get; set; }

		/// <summary>Gets or sets the email address.</summary>
		/// <value>The email address.</value>
		[EmailAddress]
		[Required(ErrorMessage = "The email address is required")]
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		/// <value>
		/// The phone number.
		/// </value>
		[Phone]
		[Required(ErrorMessage = "The phone number is required")]
		public string PhoneNumber { get; set; }


		/// <summary>Gets or sets the password.</summary>
		/// <value>The password.</value>
		[PasswordPropertyText]
		[Required(ErrorMessage = "The password is required")]
		public string Password { get; set; }
	}
}