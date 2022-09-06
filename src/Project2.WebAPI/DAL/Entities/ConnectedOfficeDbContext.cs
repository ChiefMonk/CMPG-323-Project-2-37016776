using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Project2.WebAPI.DAL.Entities
{

	/// <summary>
	/// ConnectedOfficeDbContext class
	/// </summary>
	public class ConnectedOfficeDbContext : IdentityDbContext<EntitySystemUser>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectedOfficeDbContext"/> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public ConnectedOfficeDbContext(DbContextOptions<ConnectedOfficeDbContext> options) : base(options)
		{

		}

		/// <summary>
		/// Gets or sets the category.
		/// </summary>
		/// <value>
		/// The category.
		/// </value>
		public virtual DbSet<EntityCategory> Category { get; set; }
		/// <summary>
		/// Gets or sets the device.
		/// </summary>
		/// <value>
		/// The device.
		/// </value>
		public virtual DbSet<EntityDevice> Device { get; set; }
		/// <summary>
		/// Gets or sets the zone.
		/// </summary>
		/// <value>
		/// The zone.
		/// </value>
		public virtual DbSet<EntityZone> Zone { get; set; }

		/// <summary>
		/// Gets or sets the user session.
		/// </summary>
		/// <value>
		/// The user session.
		/// </value>
		public virtual DbSet<EntityUserSession> UserSession { get; set; }

		/// <summary>
		/// Override this method to further configure the model that was discovered by convention from the entity types
		/// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
		/// and re-used for subsequent instances of your derived context.
		/// </summary>
		/// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
		/// define extension methods on this object that allow you to configure aspects of the model that are specific
		/// to a given database.</param>
		/// <remarks>
		/// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
		/// then this method will not be run.
		/// </remarks>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//modelBuilder.Ignore<EntityCategory>();
			//modelBuilder.Ignore<EntityZone>();
			//modelBuilder.Ignore<EntityDevice>();
		}
	}
}
