using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project2.WebAPI.Utils.Dtos;

namespace Project2.WebAPI.DAL.Services.Category
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICategoryService
	{
		/// <summary>
		/// Gets the category by identifier asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		ValueTask<DtoCategory> GetCategoryByIdAsync(Guid id);

		/// <summary>
		/// Gets all category collection asynchronous.
		/// </summary>
		/// <returns></returns>
		ValueTask<IList<DtoCategory>> GetAllCategoryCollectionAsync();

		/// <summary>
		/// Creates the category asynchronous.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		ValueTask<DtoCategory> CreateCategoryAsync(DtoCategory category);

		/// <summary>
		/// Updates the category asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		ValueTask<DtoCategory> UpdateCategoryAsync(Guid id, DtoCategory category);

		/// <summary>
		/// Deletes the category asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		ValueTask<Guid> DeleteCategoryAsync(Guid id);
	}
}
