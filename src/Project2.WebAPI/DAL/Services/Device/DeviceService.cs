using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project2.WebAPI.DAL.Entities;
using Project2.WebAPI.Utils.Dtos;
using Project2.WebAPI.Utils.Exceptions;
using Project2.WebAPI.Utils.Session;

namespace Project2.WebAPI.DAL.Services.Device
{

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Project2.WebAPI.DAL.Services.DataServiceBase" />
	/// <seealso cref="Project2.WebAPI.DAL.Services.Device.IDeviceService" />
	public class DeviceService : DataServiceBase, IDeviceService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceService" /> class.
		/// </summary>
		/// <param name="webSettings">The web settings.</param>
		/// <param name="officeDbContext">The office database context.</param>
		/// <param name="session">The session.</param>
		public DeviceService(
			IWebApiSettings webSettings, 
			ConnectedOfficeDbContext officeDbContext, 
			IUserSession session) : base(webSettings, officeDbContext, session)
		{

		}

		/// <summary>
		/// Gets the device by identifier asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">
		/// The device-id specified is not valid (id = '{id}')
		/// or
		/// No device with id = '{id}' has been found
		/// </exception>
		public async ValueTask<DtoDevice> GetDeviceByIdAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					throw new MyWebApiException(HttpStatusCode.BadRequest, $"The device-id specified is not valid (id = '{id}')");

				var entity = await OfficeDbContext.Device
					.AsNoTracking()
					.FirstOrDefaultAsync(e => e.DeviceId == id);

				if (entity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No device with id = '{id}' has been found");

				return entity.ToDtoDevice();
			}
			catch (MyWebApiException ex)
			{
				Console.WriteLine(ex);
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		/// <summary>
		/// Gets all device collection asynchronous.
		/// </summary>
		/// <returns></returns>
		public async ValueTask<IList<DtoDevice>> GetAllDeviceCollectionAsync()
		{
			try
			{
				var entityList = await OfficeDbContext.Device.AsNoTracking().ToListAsync();
				return entityList.ToDtoDeviceCollection();
			}
			catch (MyWebApiException ex)
			{
				Console.WriteLine(ex);
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		/// <summary>
		/// Creates the device asynchronous.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">
		/// The device-id specified is not valid (id = '{device.Id}')
		/// or
		/// A device already exists with id = '{device.Id}'
		/// </exception>
		public async ValueTask<DtoDevice> CreateDeviceAsync(DtoDevice device)
		{
			try
			{
				if (device.Id == Guid.Empty)
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						$"The device-id specified is not valid (id = '{device.Id}')");

				var currentEntity = await OfficeDbContext.Device.AsTracking().FirstOrDefaultAsync(e => e.DeviceId == device.Id);

				if (currentEntity != null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"A device already exists with id = '{device.Id}'");

				var entity = device.ToEntityDevice();
				await OfficeDbContext.Device.AddAsync(entity);

				await OfficeDbContext.SaveChangesAsync();
			}
			catch (MyWebApiException ex)
			{
				Console.WriteLine(ex);
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

			return await GetDeviceByIdAsync(device.Id);
		}

		/// <summary>
		/// Updates the device asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">
		/// The device-id specified is not valid (id = '{device.Id}')
		/// or
		/// The id specified does NOT match device-id (id = '{id}', device-id = '{device.Id}')
		/// or
		/// No device found with id = '{device.Id}'
		/// </exception>
		public async ValueTask<DtoDevice> UpdateDeviceAsync(Guid id, DtoDevice device)
		{
			if (id == Guid.Empty)
			{
				throw new MyWebApiException(
					HttpStatusCode.BadRequest,
					$"The device-id specified is not valid (id = '{device.Id}')");
			}

			if (id != device.Id)
			{
				throw new MyWebApiException(
					HttpStatusCode.BadRequest,
					$"The id specified does NOT match device-id (id = '{id}', device-id = '{device.Id}')");
			}

			try
			{
				var currentEntity = await OfficeDbContext.Device.AsTracking().FirstOrDefaultAsync(e => e.DeviceId == device.Id);

				if (currentEntity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No device found with id = '{device.Id}'");

				OfficeDbContext.Entry(device.ToEntityDevice(currentEntity)).State = EntityState.Modified;

				await OfficeDbContext.SaveChangesAsync();
			}
			catch (MyWebApiException ex)
			{
				Console.WriteLine(ex);
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

			return await GetDeviceByIdAsync(device.Id);
		}

		/// <summary>
		/// Deletes the device asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">The device-id specified is not valid (id = '{id}')</exception>
		public async ValueTask<Guid> DeleteDeviceAsync(Guid id)
		{
			if (id == Guid.Empty)
				throw new MyWebApiException(HttpStatusCode.BadRequest,
					$"The device-id specified is not valid (id = '{id}')");

			var returnId = Guid.Empty;
			try
			{
				var entity = await OfficeDbContext.Device.AsTracking().FirstOrDefaultAsync(e => e.DeviceId == id);

				if (entity != null)
				{
					OfficeDbContext.Device.Remove(entity);
					await OfficeDbContext.SaveChangesAsync();
					returnId = id;
				}
			}
			catch (MyWebApiException ex)
			{
				Console.WriteLine(ex);
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

			return returnId;
		}
	}
}
