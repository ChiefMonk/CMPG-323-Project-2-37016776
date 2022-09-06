namespace Project2.WebAPI.Utils.Session
{
	/// <summary>
	/// IWebApiSettings interface
	/// </summary>
	public interface IWebApiSettings
	{
		/// <summary>
		/// Gets or sets the SQL server connection.
		/// </summary>
		/// <value>
		/// The SQL server connection.
		/// </value>
		string SqlServerConnection { get; set; }

		/// <summary>
		/// Gets or sets the JWT audience.
		/// </summary>
		/// <value>
		/// The JWT audience.
		/// </value>
		string JwtAudience { get; set; }

		/// <summary>
		/// Gets or sets the JWT issuer.
		/// </summary>
		/// <value>
		/// The JWT issuer.
		/// </value>
		string JwtIssuer { get; set; }

		/// <summary>
		/// Gets or sets the JWT secret.
		/// </summary>
		/// <value>
		/// The JWT secret.
		/// </value>
		string JwtSecret { get; set; }
	}
}