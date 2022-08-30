using System;
using System.ComponentModel.DataAnnotations;

namespace Project2.Data.Entities
{
	public  class EntityDevice : DataEntity, IDataEntity
	{
		[Key] 
		public Guid DeviceId { get; set; }
		public string DeviceName { get; set; }
		public Guid CategoryId { get; set; }
		public Guid ZoneId { get; set; }
		public string Status { get; set; }
		public bool IsActvie { get; set; }
		public DateTime DateCreated { get; set; }
	}
}