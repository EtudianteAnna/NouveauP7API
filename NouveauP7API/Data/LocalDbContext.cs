using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NouveauP7API.Domain;

namespace NouveauP7API.Data
{
    public class LocalDbContext : IdentityDbContext<User>
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
            (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" },
                new IdentityRole() { Name = "RH", ConcurrencyStamp = "3", NormalizedName = "RH" }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DATABASE_P7;Trusted_Connection=True;MultipleActiveResultSets=true", options =>
                {
                    options.EnableRetryOnFailure(); // Ajoutez cette ligne pour activer la résilience aux erreurs transitoires
                });
            }
        }

        public DbSet<BidList> BidLists { get; set; }
        public DbSet<CurvePoints> CurvePoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<User> User { get; set; }
    }
}
