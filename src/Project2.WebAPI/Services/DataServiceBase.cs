using Project2.Data;
using Project2.WebAPI.Session;

namespace Project2.WebAPI.Services.Category
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class DataServiceBase
	{
		protected IWebApiSettings _webApiSettings;
		protected ConnectedOfficeDbContext _dbContext;
		protected IUserSession _session;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataServiceBase"/> class.
		/// </summary>
		/// <param name="webApiSettings">The web API settings.</param>
		/// <param name="dbContext">The database context.</param>
		/// <param name="session">The session.</param>
		protected DataServiceBase(IWebApiSettings webApiSettings, ConnectedOfficeDbContext dbContext, IUserSession session)
		{
			_webApiSettings = webApiSettings;
			_dbContext = dbContext;
			_session = session;
		}
	}
}
