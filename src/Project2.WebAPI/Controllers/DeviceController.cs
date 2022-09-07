using System;
using System.Collections.Generic;
using System.Linq;
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
	/// The api/devices controller
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[Route("api/devices")]
	[Authorize(Roles = ApiConstants.UserRoles.Admin)]
	[ApiController]
	public class DeviceController : ControllerBase
	{
		private const string ErrorInvalidDeviceId = "Please specify a valid device-id";
		private const string ErrorDeviceNotExit = "This device does not exist";

		private readonly ConnectedOfficeDbContext _officeDbContext;


		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceController"/> class.
		/// </summary>
		/// <param name="officeDbContext">The office database context.</param>
		public DeviceController(ConnectedOfficeDbContext officeDbContext)
		{
			_officeDbContext = officeDbContext;
		}

		/// <summary>
		/// gets all devices
		/// </summary>
		/// <returns></returns>
		[HttpGet("get-all")]
		[ProducesResponseType(typeof(IList<DtoDevice>), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<IList<DtoDevice>>> GetAllDeviceCollectionAsync()
		{
			try
			{
				var entityList = await _officeDbContext.Device.AsNoTracking().ToListAsync();
				var response = entityList.ToDtoDeviceCollection();

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
		/// gets all devices by zone id
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">No zone with id = '{id}' has been found</exception>
		[HttpGet("get-all-by-zone/{zoneId}")]
		[ProducesResponseType(typeof(IList<DtoDevice>), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<IList<DtoDevice>>> GetDeviceCollectionByZoneIdAsync(Guid zoneId)
		{
			if (zoneId == Guid.Empty)
				return BadRequest("Please specify a valid zone-id");

			try
			{
				var entityList = await _officeDbContext.Device
					.AsNoTracking()
					.Where(e => e.ZoneId == zoneId).ToListAsync();

				var response = entityList.ToDtoDeviceCollection();
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
		/// gets all devices by category id
		/// </summary>
		/// <param name="categoryId">The category identifier.</param>
		/// <returns></returns>
		[HttpGet("get-all-by-category/{categoryId}")]
		[ProducesResponseType(typeof(IList<DtoDevice>), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<IList<DtoDevice>>> GetDeviceCollectionByCategoryIdAsync(Guid categoryId)
		{
			if (categoryId == Guid.Empty)
				return BadRequest("Please specify a valid category-id");

			try
			{
				var entityList = await _officeDbContext.Device
					.AsNoTracking()
					.Where(e => e.CategoryId == categoryId).ToListAsync();

				var response = entityList.ToDtoDeviceCollection();
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
		/// gets a particular device by its id
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get/{id}", Name = "GetDevice")]
		[ProducesResponseType(typeof(DtoDevice), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoDevice>> GetDeviceByIdAsync(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest(ErrorInvalidDeviceId);

			try
			{
				var entity = await _officeDbContext.Device
					.AsNoTracking()
					.FirstOrDefaultAsync(e => e.DeviceId == id);

				if (entity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No device with id = '{id}' has been found");

				var response = entity.ToDtoDevice();

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
		/// creates a new device
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(DtoDevice), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoDevice>> CreateDeviceAsync([FromBody] DtoDevice device)
		{
			if (device.Id == Guid.Empty)
				return BadRequest(ErrorInvalidDeviceId);

			try
			{
				var exists = await DoesDeviceExistAsync(device.Id);
				if (exists)
					return BadRequest("A device with the same id already exists");

				var entity = device.ToEntityDevice();
				await _officeDbContext.Device.AddAsync(entity);
				await _officeDbContext.SaveChangesAsync();

				return Created(new Uri(Url.Link("GetDevice", new { id = device.Id })), entity.ToDtoDevice());
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
		/// updates or patches an existing device
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		[HttpPatch("update/{id}")]
		[ProducesResponseType(typeof(DtoDevice), StatusCodes.Status202Accepted)]
		public async ValueTask<ActionResult<DtoDevice>> UpdateDeviceAsync(Guid id, [FromBody] DtoDevice device)
		{
			if (id == Guid.Empty || id != device.Id)
				return BadRequest(ErrorInvalidDeviceId);

			try
			{
				var exists = await DoesDeviceExistAsync(id);
				if (!exists)
					return BadRequest(ErrorDeviceNotExit);

				var entity = await _officeDbContext.Device.AsTracking().FirstOrDefaultAsync(e => e.DeviceId == device.Id);
				_officeDbContext.Entry(device.ToEntityDevice(entity)).State = EntityState.Modified;
				await _officeDbContext.SaveChangesAsync();

				return new ObjectResult(await GetDeviceByIdAsync(id))
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
		/// deletes an existing device
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpDelete("delete/{id}")]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
		public async ValueTask<ActionResult<Guid>> DeleteDevice(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest(ErrorInvalidDeviceId);

			try
			{
				var exists = await DoesDeviceExistAsync(id);
				if (!exists)
					return BadRequest(ErrorDeviceNotExit);

				//check if category has devices assigned
				var hasDevices = await _officeDbContext.Device.AsTracking().AnyAsync(e => e.DeviceId == id);
				if (hasDevices)
				{
					throw new MyWebApiException(HttpStatusCode.Forbidden,
						"You can not delete this device because it has devices assigned to it");
				}

				var entity = await _officeDbContext.Device.AsTracking().FirstOrDefaultAsync(e => e.DeviceId == id);

				if (entity != null)
				{
					_officeDbContext.Device.Remove(entity);
					await _officeDbContext.SaveChangesAsync();
				}

				return StatusCode(StatusCodes.Status204NoContent, "The device has been deleted successfully");
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
		/// Does the device exist.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		private async ValueTask<bool> DoesDeviceExistAsync(Guid id)
		{
			return await _officeDbContext.Device.AsTracking().AnyAsync(e => e.DeviceId == id);
		}

		#endregion
	}
}
