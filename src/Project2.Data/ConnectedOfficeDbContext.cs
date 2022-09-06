using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project2.Data.Entities;

namespace Project2.Data
{
	public class ConnectedOfficeDbContext : IdentityDbContext<EntitySystemUser>
	{
		public ConnectedOfficeDbContext(DbContextOptions<ConnectedOfficeDbContext> options) : base(options)
		{

		}

		public virtual DbSet<EntityCategory> Category { get; set; }
		public virtual DbSet<EntityDevice> Device { get; set; }
		public virtual DbSet<EntityZone> Zone { get; set; }

		public virtual DbSet<EntityUserSession> UserSession { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//modelBuilder.Ignore<EntityCategory>();
			//modelBuilder.Ignore<EntityZone>();
			//modelBuilder.Ignore<EntityDevice>();
		}
	}
}
