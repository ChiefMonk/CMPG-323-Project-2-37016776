using System;

namespace Project2.WebAPI.Session
{
	/// <summary>
	/// 
	/// </summary>
	public interface IUserSession
	{

		/// <summary>
		/// Gets or sets the name of the given.
		/// </summary>
		/// <value>
		/// The name of the given.
		/// </value>
		string GivenName { get; set; }

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		/// <value>
		/// The email address.
		/// </value>
		string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		/// <value>
		/// The phone number.
		/// </value>
		string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		string UserName { get; set; }

		/// <summary>
		/// Gets or sets the session token.
		/// </summary>
		/// <value>
		/// The session token.
		/// </value>
		Guid SessionToken { get; set; }

		/// <summary>
		/// Gets or sets the role.
		/// </summary>
		/// <value>
		/// The role.
		/// </value>
		string Role { get; set; }
	}
}
