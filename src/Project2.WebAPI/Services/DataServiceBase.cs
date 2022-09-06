using Project2.Data;
using Project2.WebAPI.Session;

namespace Project2.WebAPI.Services.Category
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class DataServiceBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataServiceBase" /> class.
		/// </summary>
		/// <param name="webSettings">The web settings.</param>
		/// <param name="officeDbContext">The office database context.</param>
		/// <param name="session">The session.</param>
		protected DataServiceBase(IWebApiSettings webSettings, ConnectedOfficeDbContext officeDbContext, IUserSession session)
		{
			WebSettings = webSettings;
			OfficeDbContext = officeDbContext;
			Session = session;
		}


		/// <summary>
		/// Gets the web API settings.
		/// </summary>
		/// <value>
		/// The web API settings.
		/// </value>
		protected IWebApiSettings WebSettings
		{
			get;
		}

		/// <summary>
		/// Gets the session.
		/// </summary>
		/// <value>
		/// The session.
		/// </value>
		protected IUserSession Session
		{
			get;
		}


		/// <summary>
		/// Gets the office database context.
		/// </summary>
		/// <value>
		/// The office database context.
		/// </value>
		protected ConnectedOfficeDbContext OfficeDbContext
		{
			get;
		}
	}
}
