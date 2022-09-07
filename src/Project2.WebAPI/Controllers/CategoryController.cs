using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2.WebAPI.DAL;
using Project2.WebAPI.DAL.Converters;
using Project2.WebAPI.DAL.Dtos;
using Project2.WebAPI.Utils;
using Project2.WebAPI.Utils.Exceptions;

namespace Project2.WebAPI.Controllers
{
    /// <summary>
    /// The api/categories controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/categories")]
	[Authorize(Roles = ApiConstants.UserRoles.Admin)]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private const string ErrorInvalidCategoryId = "Please specify a valid category-id";
		private const string ErrorCategoryNotExit = "This category does not exist";

		private readonly ConnectedOfficeDbContext _officeDbContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="CategoryController" /> class.
		/// </summary>
		/// <param name="officeDbContext">The office database context.</param>
		public CategoryController(ConnectedOfficeDbContext officeDbContext)
		{
			_officeDbContext = officeDbContext;
		}

		/// <summary>
		/// gets all categories
		/// </summary>
		/// <returns></returns>
		[HttpGet("get-all")]
		[ProducesResponseType(typeof(IList<DtoCategory>), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<IList<DtoCategory>>> GetAllCategoryCollectionAsync()
		{
			try
			{
				var entityList = await _officeDbContext.Category.AsNoTracking().ToListAsync();
				var response = entityList.ToDtoCategoryCollection();

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
		/// gets a particular category by its id
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get/{id}", Name = "GetCategory")]
		[ProducesResponseType(typeof(DtoCategory), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<DtoCategory>> GetCategoryByIdAsync(Guid id)
		{
			if(id == Guid.Empty)
				return BadRequest(ErrorInvalidCategoryId);

			try
			{
				var entity = await _officeDbContext.Category
					.AsNoTracking()
					.FirstOrDefaultAsync(e => e.CategoryId == id);

				if (entity == null)
					throw new MyWebApiException(HttpStatusCode.NotFound, $"No category with id = '{id}' has been found");

				var response = entity.ToDtoCategory();

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
		/// gets the number of zones with devices linked to a category
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet("get-num-of-zones-by-category/{id}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public async ValueTask<ActionResult<int>> GetNumberOfZonesByCategoryIdAsync(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest(ErrorInvalidCategoryId);

			try
			{
				var zoneCount = await (from z in _officeDbContext.Zone.AsNoTracking()
					join d in _officeDbContext.Device.AsNoTracking() on z.ZoneId equals d.ZoneId
					where d.CategoryId == id
					select new
					{
						z.ZoneId
					}).CountAsync();

				return Ok(zoneCount);
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
		/// creates a new category
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(DtoCategory), StatusCodes.Status201Created)]
		public async ValueTask<ActionResult<DtoCategory>> CreateCategoryAsync([FromBody] DtoCategory category)
		{
			if (category.Id == Guid.Empty)
				return BadRequest(ErrorInvalidCategoryId);

			try
			{
				var exists = await DoesCategoryExistAsync(category.Id);
				if (exists)
					return BadRequest("A category with the same id already exists");

				var entity = category.ToEntityCategory();
				await _officeDbContext.Category.AddAsync(entity);
				await _officeDbContext.SaveChangesAsync();

				return Created(new Uri(Url.Link("GetCategory", new { id = category.Id })), category.ToEntityCategory());
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
		/// updates or patches an existing category
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		[HttpPatch("update/{id}")]
		[ProducesResponseType(typeof(DtoCategory), StatusCodes.Status202Accepted)]
		public async ValueTask<ActionResult<DtoCategory>> UpdateCategoryAsync(Guid id, [FromBody] DtoCategory category)
		{
			if (id == Guid.Empty || id != category.Id)
				return BadRequest(ErrorInvalidCategoryId);

			try
			{
				var exists = await DoesCategoryExistAsync(id);
				if (!exists)
					return BadRequest(ErrorCategoryNotExit);

				var entity = await _officeDbContext.Category.AsTracking().FirstOrDefaultAsync(e => e.CategoryId == category.Id);
				_officeDbContext.Entry(category.ToEntityCategory(entity)).State = EntityState.Modified;
				await _officeDbContext.SaveChangesAsync();

				return new ObjectResult(await GetCategoryByIdAsync(id))
				{
					StatusCode = StatusCodes.Status202Accepted
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
		/// deletes an existing category if no linked devices
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpDelete("delete/{id}")]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
		public async ValueTask<ActionResult<Guid>> DeleteCategory(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest(ErrorInvalidCategoryId);

			try
			{
				var exists = await DoesCategoryExistAsync(id);
				if (!exists)
					return BadRequest(ErrorCategoryNotExit);

				//check if category has devices assigned
				var hasDevices = await _officeDbContext.Device.AsTracking().AnyAsync(e => e.CategoryId == id);
				if (hasDevices)
				{
					throw new MyWebApiException(HttpStatusCode.Forbidden,
						"You can not delete this category because it has devices assigned to it");
				}

				var entity = await _officeDbContext.Category.AsTracking().FirstOrDefaultAsync(e => e.CategoryId == id);

				if (entity != null)
				{
					_officeDbContext.Category.Remove(entity);
					await _officeDbContext.SaveChangesAsync();
				}

				return StatusCode(StatusCodes.Status204NoContent, "The category has been deleted successfully");
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

		#region Privates

		/// <summary>
		/// Does the category exist asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		private async ValueTask<bool> DoesCategoryExistAsync(Guid id)
		{
			return await _officeDbContext.Category.AsTracking().AnyAsync(e => e.CategoryId == id);
		}

		#endregion
	}
}
