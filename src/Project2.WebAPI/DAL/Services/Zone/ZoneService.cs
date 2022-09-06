using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project2.WebAPI.DAL.Entities;
using Project2.WebAPI.Utils.Dtos;
using Project2.WebAPI.Utils.Exceptions;
using Project2.WebAPI.Utils.Session;

namespace Project2.WebAPI.DAL.Services.Zone
{

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Project2.WebAPI.DAL.Services.DataServiceBase" />
	/// <seealso cref="Project2.WebAPI.DAL.Services.Zone.IZoneService" />
	public class ZoneService : DataServiceBase, IZoneService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ZoneService" /> class.
		/// </summary>
		/// <param name="webSettings">The web settings.</param>
		/// <param name="officeDbContext">The office database context.</param>
		/// <param name="session">The session.</param>
		public ZoneService(
			IWebApiSettings webSettings, 
			ConnectedOfficeDbContext officeDbContext, 
			IUserSession session) : base(webSettings, officeDbContext, session)
		{

		}

		/// <summary>
		/// Gets the zone by identifier asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">
		/// The zone-id specified is not valid (id = '{id}')
		/// or
		/// No zone with id = '{id}' has been found
		/// </exception>
		public async ValueTask<DtoZone> GetZoneByIdAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					throw new MyWebApiException(HttpStatusCode.BadRequest, $"The zone-id specified is not valid (id = '{id}')");

				var entity = await OfficeDbContext.Zone
					.AsNoTracking()
					.FirstOrDefaultAsync(e => e.ZoneId == id);

				if (entity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No zone with id = '{id}' has been found");

				return entity.ToDtoZone();
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
		/// Gets all zone collection asynchronous.
		/// </summary>
		/// <returns></returns>
		public async ValueTask<IList<DtoZone>> GetAllZoneCollectionAsync()
		{
			try
			{
				var entityList = await OfficeDbContext.Zone.AsNoTracking().ToListAsync();
				return entityList.ToDtoZoneCollection();
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
		/// Creates the zone asynchronous.
		/// </summary>
		/// <param name="zone">The zone.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">
		/// The zone-id specified is not valid (id = '{zone.Id}')
		/// or
		/// A zone already exists with id = '{zone.Id}'
		/// </exception>
		public async ValueTask<DtoZone> CreateZoneAsync(DtoZone zone)
		{
			try
			{
				if (zone.Id == Guid.Empty)
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						$"The zone-id specified is not valid (id = '{zone.Id}')");

				var currentEntity = await OfficeDbContext.Zone.AsTracking().FirstOrDefaultAsync(e => e.ZoneId == zone.Id);

				if (currentEntity != null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"A zone already exists with id = '{zone.Id}'");

				var entity = zone.ToEntityZone();
				await OfficeDbContext.Zone.AddAsync(entity);

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

			return await GetZoneByIdAsync(zone.Id);
		}

		/// <summary>
		/// Updates the zone asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="zone">The zone.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">
		/// The zone-id specified is not valid (id = '{zone.Id}')
		/// or
		/// The id specified does NOT match zone-id (id = '{id}', zone-id = '{zone.Id}')
		/// or
		/// No zone found with id = '{zone.Id}'
		/// </exception>
		public async ValueTask<DtoZone> UpdateZoneAsync(Guid id, DtoZone zone)
		{
			if (id == Guid.Empty)
			{
				throw new MyWebApiException(
					HttpStatusCode.BadRequest,
					$"The zone-id specified is not valid (id = '{zone.Id}')");
			}

			if (id != zone.Id)
			{
				throw new MyWebApiException(
					HttpStatusCode.BadRequest,
					$"The id specified does NOT match zone-id (id = '{id}', zone-id = '{zone.Id}')");
			}

			try
			{
				var currentEntity = await OfficeDbContext.Zone.AsTracking().FirstOrDefaultAsync(e => e.ZoneId == zone.Id);

				if (currentEntity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No zone found with id = '{zone.Id}'");

				OfficeDbContext.Entry(zone.ToEntityZone(currentEntity)).State = EntityState.Modified;

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

			return await GetZoneByIdAsync(zone.Id);
		}

		/// <summary>
		/// Deletes the zone asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="Project2.WebAPI.Utils.Exceptions.MyWebApiException">The zone-id specified is not valid (id = '{id}')</exception>
		public async ValueTask<Guid> DeleteZoneAsync(Guid id)
		{
			if (id == Guid.Empty)
				throw new MyWebApiException(HttpStatusCode.BadRequest,
					$"The zone-id specified is not valid (id = '{id}')");

			var returnId = Guid.Empty;
			try
			{
				var entity = await OfficeDbContext.Zone.AsTracking().FirstOrDefaultAsync(e => e.ZoneId == id);

				if (entity != null)
				{
					OfficeDbContext.Zone.Remove(entity);
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
