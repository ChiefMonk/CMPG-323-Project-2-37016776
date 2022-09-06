namespace Project2.WebAPI.Utils.Session
{
	/// <summary>
	/// WebApiSettings class
	/// </summary>
	/// <seealso cref="IWebApiSettings" />
	public class WebApiSettings : IWebApiSettings
	{
		/// <summary>
		/// Gets or sets the SQL server connection.
		/// </summary>
		/// <value>
		/// The SQL server connection.
		/// </value>
		public string SqlServerConnection { get; set; }

		/// <summary>
		/// Gets or sets the JWT audience.
		/// </summary>
		/// <value>
		/// The JWT audience.
		/// </value>
		public string JwtAudience { get; set; }

		/// <summary>
		/// Gets or sets the JWT issuer.
		/// </summary>
		/// <value>
		/// The JWT issuer.
		/// </value>
		public string JwtIssuer { get; set; }

		/// <summary>
		/// Gets or sets the JWT secret.
		/// </summary>
		/// <value>
		/// The JWT secret.
		/// </value>
		public string JwtSecret { get; set; }
	}
}
