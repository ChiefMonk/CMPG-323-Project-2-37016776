using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project2.Data;
using Project2.WebAPI.Dtos;
using Project2.WebAPI.Exceptions;
using Project2.WebAPI.Session;

namespace Project2.WebAPI.Services.Category
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Project2.WebAPI.Services.Category.DataServiceBase" />
	/// <seealso cref="Project2.WebAPI.Services.Category.ICategoriesService" />
	public class CategoriesService : DataServiceBase, ICategoriesService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CategoriesService" /> class.
		/// </summary>
		/// <param name="webSettings">The web settings.</param>
		/// <param name="officeDbContext">The office database context.</param>
		/// <param name="session">The session.</param>
		public CategoriesService(
			IWebApiSettings webSettings, 
			ConnectedOfficeDbContext officeDbContext, 
			IUserSession session) : base(webSettings, officeDbContext, session)
		{

		}

		public async ValueTask<DtoCategory> GetCategoryByIdAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					throw new MyWebApiException(HttpStatusCode.BadRequest, $"The category-id specified is not valid (id = '{id}')");

				var entity = await OfficeDbContext.Category
					.AsNoTracking()
					.FirstOrDefaultAsync(e => e.CategoryId == id);

				if (entity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No category with id = '{id}' has been found");

				return entity.ToDtoCategory();
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

		public async ValueTask<IList<DtoCategory>> GetAllCategoryCollectionAsync()
		{
			try
			{
				var entityList = await OfficeDbContext.Category.AsNoTracking().ToListAsync();
				return entityList.ToDtoCategoryCollection();
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

		public async ValueTask<DtoCategory> CreateCategoryAsync(DtoCategory category)
		{
			try
			{
				if (category.Id == Guid.Empty)
					throw new MyWebApiException(HttpStatusCode.BadRequest,
						$"The category-id specified is not valid (id = '{category.Id}')");

				var currentEntity = await OfficeDbContext.Category.AsTracking().FirstOrDefaultAsync(e => e.CategoryId == category.Id);

				if (currentEntity != null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"A category already exists with id = '{category.Id}'");

				var entity = category.ToEntityCategory();
				await OfficeDbContext.Category.AddAsync(entity);

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

			return await GetCategoryByIdAsync(category.Id);
		}

		public async ValueTask<DtoCategory> UpdateCategoryAsync(Guid id, DtoCategory category)
		{
			if (id == Guid.Empty)
			{
				throw new MyWebApiException(
					HttpStatusCode.BadRequest,
					$"The category-id specified is not valid (id = '{category.Id}')");
			}

			if (id != category.Id)
			{
				throw new MyWebApiException(
					HttpStatusCode.BadRequest,
					$"The id specified does NOT match category-id (id = '{id}', category-id = '{category.Id}')");
			}

			try
			{
				var currentEntity = await OfficeDbContext.Category.AsTracking().FirstOrDefaultAsync(e => e.CategoryId == category.Id);

				if (currentEntity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No category found with id = '{category.Id}'");

				OfficeDbContext.Entry(category.ToEntityCategory(currentEntity)).State = EntityState.Modified;

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

			return await GetCategoryByIdAsync(category.Id);
		}

		public async ValueTask<Guid> DeleteCategoryAsync(Guid id)
		{
			if (id == Guid.Empty)
				throw new MyWebApiException(HttpStatusCode.BadRequest,
					$"The category-id specified is not valid (id = '{id}')");

			var returnId = Guid.Empty;
			try
			{
				var entity = await OfficeDbContext.Category.AsTracking().FirstOrDefaultAsync(e => e.CategoryId == id);

				if (entity != null)
				{
					OfficeDbContext.Category.Remove(entity);
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
