using Project2.Data;
using Project2.WebAPI.Session;

namespace Project2.WebAPI.Services.Category
{
	public abstract class DataServiceBase
	{
		protected ConnectedOfficeDbContext _dbContext;
		protected IUserSession _session;

		protected DataServiceBase(ConnectedOfficeDbContext dbContext, IUserSession session)
		{
			_dbContext = dbContext;
			_session = session;
		}
	}
}
