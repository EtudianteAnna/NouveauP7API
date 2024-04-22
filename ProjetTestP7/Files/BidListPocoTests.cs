using NouveauP7API.Models;
using System.Text.Json;
using Xunit;

namespace ProjetTestP7.Files
{
    public class BidListPocoTests
    {
        [Fact]
        public void GetBidListJson()
        {
            // Générer des valeurs aléatoires pour les propriétés
            var random = new Random();
            var bidListId = random.Next(1, 1001);
            var bidQuantity = random.NextDouble() * 100;
            var askQuantity = random.NextDouble() * 100;
            double bid = random.NextDouble() * 50;
            double ask = random.NextDouble() * 50;
            var bidListDate = new DateTime(random.Next(2020, 2025), random.Next(1, 13), random.Next(1, 29));

            var BidList = new BidList
            {
                BidListId = bidListId,
                Account = "Account1",
                BidType = "Type1",
                BidQuantity = bidQuantity,
                AskQuantity = askQuantity,
                Bid = bid,
                Ask = ask,
                Benchmark = "Benchmark1",
                BidListDate = bidListDate,
                Commentary = "Commentary1",
                BidSecurity = "Security1",
                BidStatus = "Status1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "Creator1",
                CreationDate = new DateTime(2023, 4, 21),
                RevisionName = "Reviser1",
                RevisionDate = new DateTime(2023, 4, 22),
                DealName = "Deal1",
                DealType = "Type1",
                SourceListId = "SourceList1",
                Side = "Side1",
                
            };
            var json = JsonSerializer.Serialize(BidList);
            Assert.NotNull(json);
        }
    }
}
