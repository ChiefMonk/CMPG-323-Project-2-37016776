using Microsoft.EntityFrameworkCore;
using Project2.Data.Entities;


namespace Project2.Data
{
	public partial class ConnectedOfficeDbContext : DbContext
	{
		public ConnectedOfficeDbContext()
		{
		}

		public ConnectedOfficeDbContext(DbContextOptions<ConnectedOfficeDbContext> options)
			: base(options)
		{
		}

		public virtual DbSet<EntityCategory> Category { get; set; }
		public virtual DbSet<EntityDevice> Device { get; set; }
		public virtual DbSet<EntityZone> Zone { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
				optionsBuilder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ConnectedOffice;Data Source=BLUESCORED002\\SQLEXPRESS");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EntityCategory>(entity =>
			{
				entity.Property(e => e.CategoryId)
					.HasColumnName("CategoryID")
					.ValueGeneratedNever();

				entity.Property(e => e.CategoryName).IsRequired();

				entity.Property(e => e.DateCreated)
					.HasColumnType("datetime")
					.HasDefaultValueSql("(getdate())");
			});

			modelBuilder.Entity<EntityDevice>(entity =>
			{
				entity.Property(e => e.DeviceId)
					.HasColumnName("DeviceID")
					.ValueGeneratedNever();

				entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

				entity.Property(e => e.DateCreated)
					.HasColumnType("datetime")
					.HasDefaultValueSql("(getdate())");

				entity.Property(e => e.ZoneId).HasColumnName("ZoneID");
			});

			modelBuilder.Entity<EntityZone>(entity =>
			{
				entity.Property(e => e.ZoneId)
					.HasColumnName("ZoneID")
					.ValueGeneratedNever();

				entity.Property(e => e.DateCreated)
					.HasColumnType("datetime")
					.HasDefaultValueSql("(getdate())");

				entity.Property(e => e.ZoneName).IsRequired();
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
