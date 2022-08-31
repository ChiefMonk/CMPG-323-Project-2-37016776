using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
	[Route("api/categories")]
	[Authorize]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesService _categoriesService;

		public CategoriesController(ICategoriesService categoriesService)
		{
			_categoriesService = categoriesService;
		}

		// GET: api/categories
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

		// GET: api/categories/5
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

		// POST: api/categories
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

		// PUT: api/categories/5
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



		// DELETE: api/categories/5
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
