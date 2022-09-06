using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2.WebAPI.DAL.Services.Zone;
using Project2.WebAPI.Utils;
using Project2.WebAPI.Utils.Dtos;
using Project2.WebAPI.Utils.Exceptions;

namespace Project2.WebAPI.Controllers
{
	/// <summary>
	/// The api/zones controller
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[Route("api/zones")]
	[Authorize(Roles = ApiConstants.UserRoles.Admin)]
	[ApiController]
	public class ZoneController : ControllerBase
	{
		private readonly IZoneService _zoneService;


		/// <summary>
		/// Initializes a new instance of the <see cref="ZoneController"/> class.
		/// </summary>
		/// <param name="zoneService">The zone service.</param>
		public ZoneController(IZoneService zoneService)
		{
			_zoneService = zoneService;
		}

		/// <summary>
		/// Gets all zone collection
		/// </summary>
		/// <returns></returns>
		[HttpGet("get-all")]
		[ProducesResponseType(typeof(IList<DtoZone>), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<IList<DtoZone>>> GetAllZoneCollectionAsync()
		{
			try
			{
				var response =  await _zoneService.GetAllZoneCollectionAsync();

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
		/// Gets a particular zone by its id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get-by-id/{id}", Name = "GetZone")]
		[ProducesResponseType(typeof(DtoZone), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoZone>> GetZoneByIdAsync(Guid id)
		{

			if(id == Guid.Empty)
				return BadRequest("Please specify a valid zone-id");

			try
			{
				var zone = await _zoneService.GetZoneByIdAsync(id);

				return Ok(zone);
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
		/// Creates a zone.
		/// </summary>
		/// <param name="zone">The zone.</param>
		/// <returns></returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(DtoZone), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoZone>> CreateZoneAsync([FromBody] DtoZone zone)
		{
			try
			{
				var response =  await _zoneService.CreateZoneAsync(zone);

				return Created(new Uri(Url.Link("GetZone", new { id = zone.Id })), response);
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
		/// Updates a zone.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="zone">The zone.</param>
		/// <returns></returns>
		[HttpPut("update/{id}")]
		[ProducesResponseType(typeof(DtoZone), StatusCodes.Status202Accepted)]
		public async ValueTask<ActionResult<DtoZone>> UpdateZoneAsync(Guid id, [FromBody] DtoZone zone)
		{
			if (id == Guid.Empty)
				return BadRequest("Please specify a valid zone-id to update");

			try
			{
				var response = await _zoneService.UpdateZoneAsync(id, zone);

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
		/// Deletes a zone.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpDelete("delete/{id}")]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
		public async ValueTask<ActionResult<Guid>> DeleteZone(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest("Please specify a valid zone-id to delete");

			try
			{
				var response =  await _zoneService.DeleteZoneAsync(id);

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
