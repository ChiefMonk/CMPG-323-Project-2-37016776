namespace Project2.WebAPI.DAL.Dtos
{

	/// <summary>
	/// 
	/// </summary>
	public class DtoUserAuthenticationResponse 
	{
		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>
		/// The user.
		/// </value>
		public DtoSystemUser User { get; set; }

		/// <summary>
		/// Gets or sets the JWT token.
		/// </summary>
		/// <value>
		/// The JWT token.
		/// </value>
		public string JwtToken { get; set; }

		/// <summary>
		/// Gets or sets the JWT expiry.
		/// </summary>
		/// <value>
		/// The JWT expiry.
		/// </value>
		public string JwtExpiry { get; set; }
	}
}