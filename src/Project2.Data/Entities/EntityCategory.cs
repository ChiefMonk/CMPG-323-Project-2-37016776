using System;
using System.ComponentModel.DataAnnotations;

namespace Project2.Data.Entities
{
	public class EntityCategory : DataEntity, IDataEntity
	{
		[Key]
		public Guid CategoryId { get; set; }

		public string CategoryName { get; set; }

		public string CategoryDescription { get; set; }

		public DateTime DateCreated { get; set; }
	}
}