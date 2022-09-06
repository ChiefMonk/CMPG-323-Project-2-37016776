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
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns>
		/// DtoUserAuthenticationResponse
		/// </returns>
		[HttpPost("login/{username}/{password}")]
		[ProducesResponseType(typeof(DtoUserAuthenticationResponse), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoUserAuthenticationResponse>> LoginUserAsync(string username, string password)
		{
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
				return BadRequest("Please enter a valid username and/or password");

			try
			{
				var response = await _securityService.LoginUserAsync(new DtoUserAuthenticationRequest
				{
					UserName = username,
					Password = password,
				});

				return Ok(response);
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}



		/// <summary>
		/// Logs out a system user.
		/// </summary>
		/// <returns></returns>
		[HttpDelete("logout")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async ValueTask<ActionResult> LogoutUserAsync()
		{
			try
			{
				await _securityService.LogoutUserAsync();
				return Ok();
			}
			catch (MyWebApiException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		/// <summary>
		/// Registers an admin system user.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpPost("register-admin")]
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
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
