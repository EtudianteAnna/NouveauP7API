using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Models
{
   
        // TODO: Map columns in data table BIDLIST with corresponding fields
        public class BidList
        {
            public int BidListId { get; set; }

            [Required]
            public string? Account { get; set; }

            [Required]
            public string? BidType { get; set; }
        [Required]
        public double? BidQuantity { get; set; }
        [Required]
        public double? AskQuantity { get; set; }
        [Required]
        public double? BidAmount { get; set; }
        [Required]
        public double? Ask { get; set; }
        [Required]
        public string? Benchmark { get; set; }
        [Required]
        public DateTime? BidListDate { get; set; }

            public string? Commentary { get; set; }

            [Required]
            public string? BidSecurity { get; set; }

            [Required]
        public string? BidStatus { get; set; }
        [Required]
        public string? Trader { get; set; }
        [Required]
            public string? Book { get; set; }
        [Required]
            public string? CreationName { get; set; }
        [Required]
            public DateTime? CreationDate { get; set; }
        [Required]
            public string? RevisionName { get; set; }
        [Required]
            public DateTime? RevisionDate { get; set; }
        [Required]
            public string? DealName { get; set; }
        [Required]
            public string? DealType { get; set; }
        [Required]
            public string? SourceListId { get; set; }
        [Required]
            public string? Side { get; set; }
        [Required]
        public string? Name { get; set; }
        }

    }

