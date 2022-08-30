using System;

namespace Project2.WebAPI.Dtos
{
	public interface IDto
	{
		Guid Id { get; set; }
		DateTime DateCreated { get; set; }
	}
}