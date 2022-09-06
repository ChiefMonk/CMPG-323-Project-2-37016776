using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2.WebAPI.DAL.Services.Device;
using Project2.WebAPI.Utils;
using Project2.WebAPI.Utils.Dtos;
using Project2.WebAPI.Utils.Exceptions;

namespace Project2.WebAPI.Controllers
{
	/// <summary>
	/// The api/devices controller
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[Route("api/devices")]
	[Authorize(Roles = ApiConstants.UserRoles.Admin)]
	[ApiController]
	public class DeviceController : ControllerBase
	{
		private readonly IDeviceService _deviceService;


		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceController"/> class.
		/// </summary>
		/// <param name="deviceService">The device service.</param>
		public DeviceController(IDeviceService deviceService)
		{
			_deviceService = deviceService;
		}

		/// <summary>
		/// Gets all device collection
		/// </summary>
		/// <returns></returns>
		[HttpGet("get-all")]
		[ProducesResponseType(typeof(IList<DtoDevice>), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<IList<DtoDevice>>> GetAllDeviceCollectionAsync()
		{
			try
			{
				var response =  await _deviceService.GetAllDeviceCollectionAsync();

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
		/// Gets a particular device by its id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get-by-id/{id}", Name = "GetDevice")]
		[ProducesResponseType(typeof(DtoDevice), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoDevice>> GetDeviceByIdAsync(Guid id)
		{

			if(id == Guid.Empty)
				return BadRequest("Please specify a valid device-id");

			try
			{
				var device = await _deviceService.GetDeviceByIdAsync(id);

				return Ok(device);
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
		/// Creates a device.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(DtoDevice), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoDevice>> CreateDeviceAsync([FromBody] DtoDevice device)
		{
			try
			{
				var response =  await _deviceService.CreateDeviceAsync(device);

				return Created(new Uri(Url.Link("GetDevice", new { id = device.Id })), response);
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
		/// Updates a device.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		[HttpPut("update/{id}")]
		[ProducesResponseType(typeof(DtoDevice), StatusCodes.Status202Accepted)]
		public async ValueTask<ActionResult<DtoDevice>> UpdateDeviceAsync(Guid id, [FromBody] DtoDevice device)
		{
			if (id == Guid.Empty)
				return BadRequest("Please specify a valid device-id to update");

			try
			{
				var response = await _deviceService.UpdateDeviceAsync(id, device);

				return new ObjectResult(response)
				{
					StatusCode = (int)HttpStatusCode.Accepted
				};
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
		/// Deletes a device.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpDelete("delete/{id}")]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
		public async ValueTask<ActionResult<Guid>> DeleteDevice(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest("Please specify a valid device-id to delete");

			try
			{
				var response =  await _deviceService.DeleteDeviceAsync(id);

				return new ObjectResult(response)
				{
					StatusCode = (int)HttpStatusCode.NoContent
				};
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
