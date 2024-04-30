using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NouveauP7API.Models;

namespace NouveauP7API.Data
{
    public class LocalDbContext : IdentityDbContext<User>
    {
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuration des propriétés supplémentaires de la table AspNetUsers
            builder.Entity<User>(entity =>
            {

                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");

            });

            // Reste de la configuration du modèle
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
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=P7BaseDeDonnées;Trusted_Connection=True;MultipleActiveResultSets=true", options =>
                {
                    options.EnableRetryOnFailure(); // Ajoutez cette ligne pour activer la résilience aux erreurs transitoires
                });
            }
        }


        public DbSet<BidList> BidLists { get; set; }
        public async Task<BidList> CreateBidListsAsync(BidList bidList)
        {
            var newBidList = new BidList
            {
                // Initialisation des propriétés à partir de bidList
                BidListId = bidList.BidListId,
                Account = bidList.Account,
                BidType = bidList.BidType,
                BidQuantity = bidList.BidQuantity,
                AskQuantity = bidList.AskQuantity,
                Bid=bidList.Bid,
                Ask = bidList.Ask,
                Benchmark = bidList.Benchmark,
                BidListDate = bidList.BidListDate,
                Commentary = bidList.Commentary,
                BidSecurity = bidList.BidSecurity,
                BidStatus = bidList.BidStatus,
                Trader = bidList.Trader,
                Book = bidList.Book,
                CreationName = bidList.CreationName,
                CreationDate = bidList.CreationDate,
                RevisionName = bidList.RevisionName,
                RevisionDate = bidList.RevisionDate,
                DealName = bidList.DealName,
                DealType = bidList.DealType,
                SourceListId = bidList.SourceListId,
                Side = bidList.Side
            };

            BidLists.Add(newBidList);
            await SaveChangesAsync();
            return newBidList;
        }

        public List<BidList> GetAllBidLists()
        {
            return BidLists.ToList();
        }

        
        public DbSet<CurvePoints> CurvePoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<User> User { get; set; }
    }
}
