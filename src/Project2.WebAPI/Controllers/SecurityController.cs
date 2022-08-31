using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2.WebAPI.Dtos;
using Project2.WebAPI.Exceptions;
using Project2.WebAPI.Services.Security;

namespace Project2WebAPI.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[Route("api/security")]
	[ApiController]
	public class SecurityController : ControllerBase
	{
		private readonly ISecurityService _securityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityController"/> class.
		/// </summary>
		/// <param name="securityService">The security service.</param>
		public SecurityController(ISecurityService securityService)
		{
			_securityService = securityService;
		}

		/// <summary>
		/// Logins a system user - admin or normal user.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>DtoUserAuthenticationResponse</returns>
		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(DtoUserAuthenticationResponse), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoUserAuthenticationResponse>> LoginUserAsync([FromBody] DtoUserAuthenticationRequest request)
		{
			try
			{
				var response = await _securityService.LoginUserAsync(request);

				return Ok(response);
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}



		/// <summary>
		/// Logs out a system user.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <returns></returns>
		[HttpDelete("logout/{username}")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async ValueTask<ActionResult> LogoutUserAsync(string username)
		{
			if (string.IsNullOrWhiteSpace(username))
				return BadRequest();

			try
			{
				await _securityService.LogoutUserAsync(username);
				return Ok();
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}


		/// <summary>
		/// Registers an admin system user.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpPost("register-admin")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(DtoUserRegistrationResponse), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoUserRegistrationResponse>> RegisterAdminUserAsync([FromBody] DtoUserRegistrationRequest request)
		{
			try
			{
				var response = await _securityService.RegisterAdminUserAsync(request);

				return Created(new Uri(Url.Link("GetUser", new { id = response.User.Id })), response);
			}
			catch (MyWebApiException ex)
			{
					return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Registers a normal system user.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>DtoUserRegistrationResponse</returns>
		[HttpPost("register-user")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(DtoUserRegistrationResponse), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoUserRegistrationResponse>> RegisterNormalUserAsync([FromBody] DtoUserRegistrationRequest request)
		{
			try
			{
				var response = await _securityService.RegisterNormalUserAsync(request);

				return Created(new Uri(Url.Link("GetUser", new { id = response.User.Id })), response);
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}
	}
}
