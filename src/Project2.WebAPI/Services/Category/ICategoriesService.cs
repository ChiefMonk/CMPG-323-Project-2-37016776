using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project2.WebAPI.Dtos;

namespace Project2.WebAPI.Services.Category
{
	public interface ICategoriesService
	{
		ValueTask<DtoCategory> GetCategoryByIdAsync(Guid id);

		ValueTask<IList<DtoCategory>> GetAllCategoryCollectionAsync();

		ValueTask<DtoCategory> CreateCategoryAsync(DtoCategory category);

		ValueTask<DtoCategory> UpdateCategoryAsync(Guid id, DtoCategory category);

		ValueTask<Guid> DeleteCategoryAsync(Guid id);
	}
}
