using System.ComponentModel.DataAnnotations;

namespace Project2.WebAPI.DAL.Dtos
{
	/// <summary>
	/// 
	/// </summary>
	public class DtoSystemUser
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public string Id { get; set; }


		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		[Required(ErrorMessage = "User Name is required")] 
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		/// <value>
		/// The email address.
		/// </value>
		[Required(ErrorMessage = "Email Address is required"), EmailAddress]
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		/// <value>
		/// The phone number.
		/// </value>
		[Required(ErrorMessage = "Phone Number is required")] 
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the UTC offset.
		/// </summary>
		/// <value>
		/// The UTC offset.
		/// </value>
		public double UtcOffset { get; set; }

		/// <summary>
		/// Gets or sets the name of the role.
		/// </summary>
		/// <value>
		/// The name of the role.
		/// </value>
		public string RoleName { get; set; }
	}
}