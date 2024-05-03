using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Models
{

    // TODO: Map columns in data table BIDLIST with corresponding fields
    public class BidList
    {
        internal string userId;

        public int BidListId { get; set; }


        public string? Account { get; set; }


        public string BidType { get; set; }

        public int? BidQuantity { get; set; }

        public decimal? Bid { get; set; }

        public decimal? Ask { get; set; }

        public int? AskQuantity { get; set; }

        public decimal? BidAmount { get; set; }

        public string? Benchmark { get; set; }

        public DateTime? BidListDate { get; set; }

        public string? Commentary { get; set; }


        public string BidSecurity { get; set; }


        public string? BidStatus { get; set; }

        public string? Trader { get; set; }

        public string? Book { get; set; }

        public string? CreationName { get; set; }

        public DateTime? CreationDate { get; set; }

        public string? RevisionName { get; set; }

        public DateTime? RevisionDate { get; set; }

        public string? DealName { get; set; }

        public string? DealType { get; set; }

        public string? SourceListId { get; set; }

        public string? Side { get; set; }

        public string? Name { get; set; }
    }

}

