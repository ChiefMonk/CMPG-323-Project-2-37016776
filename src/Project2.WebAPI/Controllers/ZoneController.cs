using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2.WebAPI.DAL;
using Project2.WebAPI.DAL.Converters;
using Project2.WebAPI.DAL.Dtos;
using Project2.WebAPI.Utils;
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
		private const string ErrorInvalidZoneId = "Please specify a valid zone-id";
		private const string ErrorZoneNotExit = "This zone does not exist";

		private readonly ConnectedOfficeDbContext _officeDbContext;


		/// <summary>
		/// Initializes a new instance of the <see cref="ZoneController" /> class.
		/// </summary>
		/// <param name="officeDbContext">The office database context.</param>
		public ZoneController(ConnectedOfficeDbContext officeDbContext)
		{
			_officeDbContext = officeDbContext;
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
				var entityList = await _officeDbContext.Zone.AsNoTracking().ToListAsync();
				var response = entityList.ToDtoZoneCollection();

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
		[HttpGet("get/{id}", Name = "GetZone")]
		[ProducesResponseType(typeof(DtoZone), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoZone>> GetZoneByIdAsync(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest(ErrorInvalidZoneId);

			try
			{
				var entity = await _officeDbContext.Zone
					.AsNoTracking()
					.FirstOrDefaultAsync(e => e.ZoneId == id);

				if (entity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No zone with id = '{id}' has been found");

				var response = entity.ToDtoZone();

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
		/// Creates a zone.
		/// </summary>
		/// <param name="zone">The zone.</param>
		/// <returns></returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(DtoZone), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoZone>> CreateZoneAsync([FromBody] DtoZone zone)
		{
			if (zone.Id == Guid.Empty)
				return BadRequest(ErrorInvalidZoneId);

			try
			{
				var exists = await DoesZoneExistAsync(zone.Id);
				if (exists)
					return BadRequest("A zone with the same id already exists");

				var entity = zone.ToEntityZone();
				await _officeDbContext.Zone.AddAsync(entity);
				await _officeDbContext.SaveChangesAsync();

				return Created(new Uri(Url.Link("GetZone", new { id = zone.Id })), zone.ToEntityZone());
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
		[HttpPatch("update/{id}")]
		[ProducesResponseType(typeof(DtoZone), StatusCodes.Status202Accepted)]
		public async ValueTask<ActionResult<DtoZone>> UpdateZoneAsync(Guid id, [FromBody] DtoZone zone)
		{
			if (id == Guid.Empty)
				return BadRequest(ErrorInvalidZoneId);

			try
			{
				var exists = await DoesZoneExistAsync(id);
				if (!exists)
					return BadRequest(ErrorZoneNotExit);

				var entity = await _officeDbContext.Zone.AsTracking().FirstOrDefaultAsync(e => e.ZoneId == zone.Id);
				_officeDbContext.Entry(zone.ToEntityZone(entity)).State = EntityState.Modified;
				await _officeDbContext.SaveChangesAsync();

				return new ObjectResult(await GetZoneByIdAsync(id))
				{
					StatusCode = StatusCodes.Status202Accepted
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
				return BadRequest(ErrorInvalidZoneId);

			try
			{
				var exists = await DoesZoneExistAsync(id);
				if (!exists)
					return BadRequest(ErrorZoneNotExit);

				//check if zone has devices assigned
				var hasDevices = await _officeDbContext.Device.AsTracking().AnyAsync(e => e.ZoneId == id);
				if (hasDevices)
				{
					throw new MyWebApiException(HttpStatusCode.Forbidden,
						"You can not delete this zone because it has devices assigned to it");
				}

				var entity = await _officeDbContext.Zone.AsTracking().FirstOrDefaultAsync(e => e.ZoneId == id);

				if (entity != null)
				{
					_officeDbContext.Zone.Remove(entity);
					await _officeDbContext.SaveChangesAsync();
				}

				return StatusCode(StatusCodes.Status204NoContent, "The category has been deleted successfully");
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

		#region Privates

		/// <summary>
		/// Does the zone exist asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		private async ValueTask<bool> DoesZoneExistAsync(Guid id)
		{
			return await _officeDbContext.Zone.AsTracking().AnyAsync(e => e.ZoneId == id);
		}

		#endregion
	}
}
