using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Data.Entities
{
	[Table("Category")]
	public class EntityCategory : DataEntity, IDataEntity
	{
		[Key]
		public Guid CategoryId { get; set; }

		public string CategoryName { get; set; }

		public string CategoryDescription { get; set; }

		public DateTime DateCreated { get; set; }
	}
}