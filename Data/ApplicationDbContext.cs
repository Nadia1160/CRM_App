using CRM_APP.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_APP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Activity> Activities => Set<Activity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Existing cascade delete for Customer -> Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Lead -> Activities (cascade delete if lead deleted)
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Lead)
                .WithMany(l => l.Activities)
                .HasForeignKey(a => a.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            // Account -> Activities
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Account)
                .WithMany(ac => ac.Activities)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}