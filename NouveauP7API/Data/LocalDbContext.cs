using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NouveauP7API.Domain;

namespace NouveauP7API.Data
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext()
        {
        }

        public LocalDbContext(DbContextOptions options) : base(options)
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

        public DbSet<BidList> BidLists { get; set; }
        public DbSet<CurvePoints> CurvePoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }
       public   DbSet<User> Users { get; set; }

    }

}
