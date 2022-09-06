using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Project2.WebAPI.DAL.Services.Security;
using Project2.WebAPI.Utils.Session;

namespace Project2.WebAPI.Utils.Middleware
{

	/// <summary>
	/// SessionMiddleware class
	/// </summary>
	public class SessionMiddleware
	{
		private readonly RequestDelegate _next;


		/// <summary>
		/// Initializes a new instance of the <see cref="SessionMiddleware"/> class.
		/// </summary>
		/// <param name="next">The next.</param>
		public SessionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		/// <summary>
		/// Invokes the specified HTTP context.
		/// </summary>
		/// <param name="httpContext">The HTTP context.</param>
		/// <param name="securityService">The security service.</param>
		/// <param name="session">The session.</param>
		public async Task Invoke(HttpContext httpContext, ISecurityService securityService, IUserSession session)
		{
			if (httpContext?.User?.Identity != null && httpContext.User.Identity.IsAuthenticated)
			{
				if (httpContext.User.Identity is ClaimsIdentity identity && identity.Claims != null)
				{
					session.UserName = identity.FindFirst(ApiConstants.UserClaims.UserName)?.Value;
					session.GivenName = identity.FindFirst(ApiConstants.UserClaims.GivenName)?.Value;
					session.EmailAddress = identity.FindFirst(ApiConstants.UserClaims.EmailAddress)?.Value;
					session.PhoneNumber = identity.FindFirst(ApiConstants.UserClaims.PhoneNumber)?.Value;
					var sessionKey = identity.FindFirst(ApiConstants.UserClaims.SessionToken)?.Value;
					session.SessionToken = Guid.Empty;
					if (!string.IsNullOrWhiteSpace(sessionKey))
						session.SessionToken = Guid.Parse(sessionKey);
					session.Role = identity.FindFirst(ApiConstants.UserClaims.Role)?.Value;
				}

				if (session.SessionToken == Guid.Empty || !await securityService.IsUserSessionValidAsync())
				{
					httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
					httpContext.Response.ContentType = "text/plain";
					await httpContext.Response.WriteAsync( "Your session expired. Please re-authenticate and try again", Encoding.UTF8);
					return;
				}
			}

			await _next(httpContext);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public static class SessionMiddlewareExtensions
	{
		/// <summary>
		/// Uses the custom session middleware.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns></returns>
		public static IApplicationBuilder UseCustomSessionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<SessionMiddleware>();
		}
	}
}
