using System;

namespace Project2.WebAPI.Dtos
{
	public abstract class Dto
	{
		public Guid Id { get; set; }

		public DateTime DateCreated { get; set; }
	}
}
