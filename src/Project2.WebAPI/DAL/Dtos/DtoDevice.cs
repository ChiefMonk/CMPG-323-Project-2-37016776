using System;
using System.ComponentModel.DataAnnotations;

namespace Project2.WebAPI.DAL.Dtos
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Dto" />
	/// <seealso cref="IDto" />
	public class DtoDevice : Dto, IDto
	{
		/// <summary>
		/// Gets or sets the name of the device.
		/// </summary>
		/// <value>
		/// The name of the device.
		/// </value>
		[Required(ErrorMessage = "Device Name is required")] 
		public string DeviceName { get; set; }
		/// <summary>
		/// Gets or sets the category identifier.
		/// </summary>
		/// <value>
		/// The category identifier.
		/// </value>
		[Required(ErrorMessage = "Category Id is required")] 
		public Guid CategoryId { get; set; }
		/// <summary>
		/// Gets or sets the zone identifier.
		/// </summary>
		/// <value>
		/// The zone identifier.
		/// </value>
		[Required(ErrorMessage = "Zone Id is required")] 
		public Guid ZoneId { get; set; }
		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[Required(ErrorMessage = "Status is required")] 
		public string Status { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsActive { get; set; }
		
	}
}