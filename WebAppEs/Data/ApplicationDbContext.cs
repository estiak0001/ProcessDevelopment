using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppEs.Entity;
using WebAppEs.Models;

namespace WebAppEs.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
		{

		}

		public DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }
		public DbSet<NavigationMenu> NavigationMenu { get; set; }
		public DbSet<MobileRNDFaultsEntry> MobileRNDFaultsEntry { get; set; }
		public DbSet<MobileRNDPartsModels> MobileRNDPartsModels { get; set; }
		public DbSet<MobileRNDFaultDetails> MobileRNDFaultDetails { get; set; }
		
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<ApplicationUser>()
				.Property(e => e.Department)
				.HasMaxLength(100);

			builder.Entity<ApplicationUser>()
				.Property(e => e.Name)
				.HasMaxLength(100);

			builder.Entity<ApplicationUser>()
				.Property(e => e.EmployeeID)
				.HasMaxLength(100);

			builder.Entity<RoleMenuPermission>()
			.HasKey(c => new { c.RoleId, c.NavigationMenuId});
			

			base.OnModelCreating(builder);
		}
	}
}