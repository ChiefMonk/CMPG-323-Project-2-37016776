using System;
using System.ComponentModel.DataAnnotations;

namespace Project2.Data.Entities
{
	public class EntityZone : DataEntity, IDataEntity
	{
		[Key]
		public Guid ZoneId { get; set; }
		public string ZoneName { get; set; }
		public string ZoneDescription { get; set; }
		public DateTime DateCreated { get; set; }
	}
}