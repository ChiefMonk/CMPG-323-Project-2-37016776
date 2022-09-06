using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2.WebAPI.DAL.Services.Security;
using Project2.WebAPI.Utils;
using Project2.WebAPI.Utils.Dtos;
using Project2.WebAPI.Utils.Exceptions;

namespace Project2.WebAPI.Controllers
{
	/// <summary>
	/// The api/security controller
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[Route("api/security")]
	[Authorize(Roles = ApiConstants.UserRoles.AdminOrUser)]
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
		/// logs in a system user - admin or normal user.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns>
		/// DtoUserAuthenticationResponse
		/// </returns>
		[HttpGet("login/{username}/{password}")]
		[AllowAnonymous]
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
		/// logs out a system user.
		/// </summary>
		/// <returns></returns>
		[HttpDelete("logout")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
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
		/// registers an admin system user.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpPost("register/admin")]
		[AllowAnonymous]
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
		/// registers a normal system user.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>DtoUserRegistrationResponse</returns>
		[HttpPost("register/user")]
		[AllowAnonymous]
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


		/// <summary>
		/// gets a particular system user by its id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get-user-by-id/{id}", Name = "GetUser")]
		[ProducesResponseType(typeof(DtoSystemUser), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoSystemUser>> GetSystemUserByIdAsync(Guid id)
		{

			if (id == Guid.Empty)
				return BadRequest("Please specify a valid system user id");

			try
			{
				var user = await _securityService.GetSystemUserByIdAsync(id);

				return Ok(user);
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
