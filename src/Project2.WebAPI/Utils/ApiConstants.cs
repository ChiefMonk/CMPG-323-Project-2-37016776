namespace Project2.WebAPI.Aut
{
	/// <summary>
	/// 
	/// </summary>
	public static class ApiConstants
	{
		/// <summary>
		/// 
		/// </summary>
		public static class UserRoles
		{
			/// <summary>The admin</summary>
			public const string Admin = "Admin";

			/// <summary>The user</summary>
			public const string User = "User";
		}

		/// <summary>
		/// 
		/// </summary>
		public static class UserClaims
		{
			/// <summary>
			/// The user name
			/// </summary>
			public const string UserName = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti;
			/// <summary>
			/// The name
			/// </summary>
			public const string GivenName = "name";

			/// <summary>
			/// The email address
			/// </summary>
			public const string EmailAddress = "email";

			/// <summary>
			/// The phone number
			/// </summary>
			public const string PhoneNumber = "phone";

		
			/// <summary>
			/// The session token
			/// </summary>
			public const string SessionToken = "x-auth";

			/// <summary>
			/// The role
			/// </summary>
			public const string Role = "role";
		}
	}

}