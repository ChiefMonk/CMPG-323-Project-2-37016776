using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.WebAPI.DAL.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Table("Device")]
	public  class EntityDevice
	{
		/// <summary>
		/// Gets or sets the device identifier.
		/// </summary>
		/// <value>
		/// The device identifier.
		/// </value>
		[Key] 
		public Guid DeviceId { get; set; }
		/// <summary>
		/// Gets or sets the name of the device.
		/// </summary>
		/// <value>
		/// The name of the device.
		/// </value>
		public string DeviceName { get; set; }
		/// <summary>
		/// Gets or sets the category identifier.
		/// </summary>
		/// <value>
		/// The category identifier.
		/// </value>
		public Guid CategoryId { get; set; }
		/// <summary>
		/// Gets or sets the zone identifier.
		/// </summary>
		/// <value>
		/// The zone identifier.
		/// </value>
		public Guid ZoneId { get; set; }
		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		public string Status { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is actvie.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is actvie; otherwise, <c>false</c>.
		/// </value>
		public bool IsActvie { get; set; }
		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>
		/// The date created.
		/// </value>
		public DateTime DateCreated { get; set; }
	}
}