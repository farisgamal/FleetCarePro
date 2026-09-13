using MVCDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCDemo.Models;

namespace MVCDemo.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=FARISGAMALALI\\SQLEXPRESS02 ; Database=FleetCarePro2 ; Trusted_Connection = true ; Encrypt = false");
        }

       

     
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
        public DbSet<ServiceCenter> ServiceCenters { get; set; } = null!;
        public DbSet<VendorService> VendorServices { get; set; } = null!;
        public DbSet<ServiceRecord> ServiceRecords { get; set; } = null!;
        public DbSet<ServiceLineItem> ServiceLineItems { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    }
}
