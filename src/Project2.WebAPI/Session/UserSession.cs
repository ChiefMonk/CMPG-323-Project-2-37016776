using System;

namespace Project2.WebAPI.Session
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Project2.WebAPI.Session.IUserSession" />
	public class UserSession : IUserSession
	{

		/// <summary>
		/// Gets or sets the name of the given.
		/// </summary>
		/// <value>
		/// The name of the given.
		/// </value>
		public string GivenName { get; set; }
		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		/// <value>
		/// The email address.
		/// </value>
		public string EmailAddress { get; set; }
		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		/// <value>
		/// The phone number.
		/// </value>
		public string PhoneNumber { get; set; }
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		public string UserName { get; set; }
		/// <summary>
		/// Gets or sets the session token.
		/// </summary>
		/// <value>
		/// The session token.
		/// </value>
		public Guid SessionToken { get; set; }
		/// <summary>
		/// Gets or sets the role.
		/// </summary>
		/// <value>
		/// The role.
		/// </value>
		public string Role { get; set; }
	}
}