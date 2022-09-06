namespace Project2.WebAPI.Utils.Dtos
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Project2.WebAPI.Utils.Dtos.Dto" />
	/// <seealso cref="Project2.WebAPI.Utils.Dtos.IDto" />
	public class DtoCategory : Dto, IDto
	{
		/// <summary>
		/// Gets or sets the name of the category.
		/// </summary>
		/// <value>
		/// The name of the category.
		/// </value>
		public string CategoryName { get; set; }

		/// <summary>
		/// Gets or sets the category description.
		/// </summary>
		/// <value>
		/// The category description.
		/// </value>
		public string CategoryDescription { get; set; }
	}
}