using System;

namespace Project2.WebAPI.DAL.Dtos
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		Guid Id { get; set; }
		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>
		/// The date created.
		/// </value>
		DateTime DateCreated { get; set; }
	}
}