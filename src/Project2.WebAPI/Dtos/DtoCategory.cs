namespace Project2.WebAPI.Dtos
{
	public class DtoCategory : Dto, IDto
	{
		public string CategoryName { get; set; }

		public string CategoryDescription { get; set; }
	}
}