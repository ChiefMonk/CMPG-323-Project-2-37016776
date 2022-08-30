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
	public class CategoriesService : DataServiceBase, ICategoriesService
	{
		public CategoriesService(ConnectedOfficeDbContext dbContext, IUserSession session):base(dbContext, session)
		{
			
		}

		public async ValueTask<DtoCategory> GetCategoryByIdAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					throw new MyWebApiException(HttpStatusCode.BadRequest, $"The category-id specified is not valid (id = '{id}')");

				var entity = await _dbContext.Category
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
				var entityList = await _dbContext.Category.AsNoTracking().ToListAsync();
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

				var currentEntity = await _dbContext.Category.AsTracking().FirstOrDefaultAsync(e => e.CategoryId == category.Id);

				if (currentEntity != null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"A category already exists with id = '{category.Id}'");

				var entity = category.ToEntityCategory();
				await _dbContext.Category.AddAsync(entity);

				await _dbContext.SaveChangesAsync();
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
				var currentEntity = await _dbContext.Category.AsTracking().FirstOrDefaultAsync(e => e.CategoryId == category.Id);

				if (currentEntity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No category found with id = '{category.Id}'");

				_dbContext.Entry(category.ToEntityCategory(currentEntity)).State = EntityState.Modified;

				await _dbContext.SaveChangesAsync();
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
				var entity = await _dbContext.Category.AsTracking().FirstOrDefaultAsync(e => e.CategoryId == id);

				if (entity != null)
				{
					_dbContext.Category.Remove(entity);
					await _dbContext.SaveChangesAsync();
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
