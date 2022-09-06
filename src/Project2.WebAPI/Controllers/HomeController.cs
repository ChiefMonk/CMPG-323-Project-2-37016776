using Microsoft.AspNetCore.Mvc;

namespace Project2.WebAPI.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	public class HomeController : ControllerBase
	{
		/// <summary>
		/// Redirects to swagger UI.
		/// </summary>
		/// <returns></returns>
		[Route(""), HttpGet]
		[ApiExplorerSettings(IgnoreApi = true)]
		public RedirectResult RedirectToSwaggerUi()
		{
			return Redirect("/swagger/");
		}

	}
}
