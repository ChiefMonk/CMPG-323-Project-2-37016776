using System;
using System.ComponentModel.DataAnnotations;

namespace Project2.WebAPI.DAL.Dtos
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class Dto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Required(ErrorMessage = "Id is required")]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>
		/// The date created.
		/// </value>
		[Required(ErrorMessage = "Date Created is required")] 
		public DateTime DateCreated { get; set; }
	}
}
