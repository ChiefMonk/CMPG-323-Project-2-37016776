using System.ComponentModel.DataAnnotations;

namespace Project2.WebAPI.DAL.Dtos
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Dto" />
	/// <seealso cref="IDto" />
	public class DtoCategory : Dto, IDto
	{
		/// <summary>
		/// Gets or sets the name of the category.
		/// </summary>
		/// <value>
		/// The name of the category.
		/// </value>
		[Required(ErrorMessage = "Category Name is required")]
		public string CategoryName { get; set; }

		/// <summary>
		/// Gets or sets the category description.
		/// </summary>
		/// <value>
		/// The category description.
		/// </value>
		[Required(ErrorMessage = "Category Description is required")]
		public string CategoryDescription { get; set; }
	}
}