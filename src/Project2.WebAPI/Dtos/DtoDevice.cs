using System;

namespace Project2.WebAPI.Dtos
{
	public  class DtoDevice : Dto, IDto
	{
		public string DeviceName { get; set; }
		public Guid CategoryId { get; set; }
		public Guid ZoneId { get; set; }
		public string Status { get; set; }
		public bool IsActive { get; set; }
		
	}
}