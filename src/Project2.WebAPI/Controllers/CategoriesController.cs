using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2.WebAPI.Aut;
using Project2.WebAPI.Dtos;
using Project2.WebAPI.Exceptions;
using Project2.WebAPI.Services.Category;

namespace Project2WebAPI.Controllers
{
	/// <summary>
	/// The api/categories controller
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[Route("api/categories")]
	[Authorize(Roles = ApiConstants.UserRoles.Admin)]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesService _categoriesService;

		/// <summary>
		/// Initializes a new instance of the <see cref="CategoriesController"/> class.
		/// </summary>
		/// <param name="categoriesService">The categories service.</param>
		public CategoriesController(ICategoriesService categoriesService)
		{
			_categoriesService = categoriesService;
		}

		/// <summary>
		/// Gets all category collection
		/// </summary>
		/// <returns></returns>
		[HttpGet("get-all")]
		[ProducesResponseType(typeof(IList<DtoCategory>), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<IList<DtoCategory>>> GetAllCategoryCollectionAsync()
		{
			try
			{
				var response =  await _categoriesService.GetAllCategoryCollectionAsync();

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
		/// Gets a particular category by its id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get-by-id/{id}", Name = "GetCategory")]
		[ProducesResponseType(typeof(DtoCategory), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoCategory>> GetCategoryByIdAsync(Guid id)
		{

			if(id == Guid.Empty)
				return BadRequest("Please specify a valid category-id");

			try
			{
				var category = await _categoriesService.GetCategoryByIdAsync(id);

				return Ok(category);
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
		/// Creates a category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(DtoCategory), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoCategory>> CreateCategoryAsync([FromBody] DtoCategory category)
		{
			try
			{
				var response =  await _categoriesService.CreateCategoryAsync(category);

				return Created(new Uri(Url.Link("GetCategory", new { id = category.Id })), response);
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
		/// Updates a category.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		[HttpPut("update/{id}")]
		[ProducesResponseType(typeof(DtoCategory), StatusCodes.Status202Accepted)]
		public async ValueTask<ActionResult<DtoCategory>> UpdateCategoryAsync(Guid id, [FromBody] DtoCategory category)
		{
			if (id == Guid.Empty)
				return BadRequest("Please specify a valid category-id to update");

			try
			{
				var response = await _categoriesService.UpdateCategoryAsync(id, category);

				return new ObjectResult(response)
				{
					StatusCode = (int)HttpStatusCode.Accepted
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
		/// Deletes a category.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpDelete("delete/{id}")]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
		public async ValueTask<ActionResult<Guid>> DeleteCategory(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest("Please specify a valid category-id to delete");

			try
			{
				var response =  await _categoriesService.DeleteCategoryAsync(id);

				return new ObjectResult(response)
				{
					StatusCode = (int)HttpStatusCode.NoContent
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
	}
}
