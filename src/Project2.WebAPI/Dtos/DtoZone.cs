namespace Project2.WebAPI.Dtos
{
	public class DtoZone : Dto, IDto
	{
		public string ZoneName { get; set; }
		public string ZoneDescription { get; set; }
	}
}