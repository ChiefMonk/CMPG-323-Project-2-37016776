using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project2.WebAPI.Utils.Dtos;

namespace Project2.WebAPI.DAL.Services.Zone
{
	/// <summary>
	/// 
	/// </summary>
	public interface IZoneService
	{
		/// <summary>
		/// Gets the zone by identifier asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		ValueTask<DtoZone> GetZoneByIdAsync(Guid id);

		/// <summary>
		/// Gets all zone collection asynchronous.
		/// </summary>
		/// <returns></returns>
		ValueTask<IList<DtoZone>> GetAllZoneCollectionAsync();

		/// <summary>
		/// Creates the zone asynchronous.
		/// </summary>
		/// <param name="zone">The zone.</param>
		/// <returns></returns>
		ValueTask<DtoZone> CreateZoneAsync(DtoZone zone);

		/// <summary>
		/// Updates the zone asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="zone">The zone.</param>
		/// <returns></returns>
		ValueTask<DtoZone> UpdateZoneAsync(Guid id, DtoZone zone);

		/// <summary>
		/// Deletes the zone asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		ValueTask<Guid> DeleteZoneAsync(Guid id);
	}
}
