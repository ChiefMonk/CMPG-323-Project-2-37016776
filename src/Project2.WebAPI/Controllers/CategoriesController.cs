using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2.WebAPI.Dtos;
using Project2.WebAPI.Exceptions;
using Project2.WebAPI.Services.Category;

namespace Project2WebAPI.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[Route("api/categories")]
	//[Authorize]
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
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}


		/// <summary>
		/// Gets the category by identifier asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get-by-id/{id}", Name = "GetCategory")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType( StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(DtoCategory), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoCategory>> GetCategoryByIdAsync(Guid id)
		{
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
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Creates the category asynchronous.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		[HttpPost("create")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Updates the category asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		[HttpPut("update/{id}")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(DtoCategory), StatusCodes.Status202Accepted)]
		public async ValueTask<ActionResult<DtoCategory>> UpdateCategoryAsync(Guid id, [FromBody] DtoCategory category)
		{
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
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}



		/// <summary>
		/// Deletes the category.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpDelete("delete/{id}")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
		public async ValueTask<ActionResult<Guid>> DeleteCategory(Guid id)
		{
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
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}
	}
}
