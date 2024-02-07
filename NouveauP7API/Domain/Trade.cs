using System.ComponentModel.DataAnnotations;
namespace NouveauP7API.Domain;


public class Trade
{
    public int TradeId { get; set; }

    [Required]
    public string? Account { get; set; }

    [Required]
    public string? AccountType { get; set; }

    [Required]
    public double? BuyQuantity { get; set; }

    [Required]
    public double? SellQuantity { get; set; }

    [Required]
    public double? BuyPrice { get; set; }

    [Required]
    public double? SellPrice { get; set; }


    [DataType(DataType.Date)]
    public DateTime? TradeDate { get; set; }

    public string? TradeSecurity { get; set; }

    [MaxLength(50)]
    public string? TradeStatus { get; set; }

    [MaxLength(50)]
    public string? Trader { get; set; }

    public string ?Benchmark { get; set; }

    public string? Book { get; set; }

   
    public string ?CreationName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? CreationDate { get; set; }

    public string ?RevisionName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? RevisionDate { get; set; }

    [Required]
        public string ?DealName { get; set; }

    [Required]
    public string? DealType { get; set; }

    [Required]
    public string? SourceListId { get; set; }

    [Required]
    public string? Side { get; set; }
}
