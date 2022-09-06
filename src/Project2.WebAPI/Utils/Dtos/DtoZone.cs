namespace Project2.WebAPI.Utils.Dtos
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Project2.WebAPI.Utils.Dtos.Dto" />
	/// <seealso cref="Project2.WebAPI.Utils.Dtos.IDto" />
	public class DtoZone : Dto, IDto
	{
		/// <summary>
		/// Gets or sets the name of the zone.
		/// </summary>
		/// <value>
		/// The name of the zone.
		/// </value>
		public string ZoneName { get; set; }
		/// <summary>
		/// Gets or sets the zone description.
		/// </summary>
		/// <value>
		/// The zone description.
		/// </value>
		public string ZoneDescription { get; set; }
	}
}