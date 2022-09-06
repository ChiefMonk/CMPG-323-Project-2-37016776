using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Data.Entities
{
	[Table("UserSession")]
	public class EntityUserSession : DataEntity, IDataEntity
	{
		[Key]
		public Guid SessionId { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? LogoutDate { get; set; }
	}
}