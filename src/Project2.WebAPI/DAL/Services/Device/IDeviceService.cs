using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project2.WebAPI.Utils.Dtos;

namespace Project2.WebAPI.DAL.Services.Device
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDeviceService
	{
		/// <summary>
		/// Gets the device by identifier asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		ValueTask<DtoDevice> GetDeviceByIdAsync(Guid id);

		/// <summary>
		/// Gets all device collection asynchronous.
		/// </summary>
		/// <returns></returns>
		ValueTask<IList<DtoDevice>> GetAllDeviceCollectionAsync();

		/// <summary>
		/// Creates the device asynchronous.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		ValueTask<DtoDevice> CreateDeviceAsync(DtoDevice device);

		/// <summary>
		/// Updates the device asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		ValueTask<DtoDevice> UpdateDeviceAsync(Guid id, DtoDevice device);

		/// <summary>
		/// Deletes the device asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		ValueTask<Guid> DeleteDeviceAsync(Guid id);
	}
}
