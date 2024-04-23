using System.ComponentModel.DataAnnotations;
namespace NouveauP7API.Models;


public class Trade
{
    public int TradeId { get; set; }

    [Required]
    public string? Account { get; set; }

    [Required]
    public string? AccountType { get; set; }

    public double? BuyQuantity { get; set; }

    
    public double? SellQuantity { get; set; }

    
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

    
    [DataType(DataType.Date)]
    public DateTime? CreationDate { get; set; }

    public string ?RevisionName { get; set; }

    
    [DataType(DataType.Date)]
    public DateTime? RevisionDate { get; set; }

    
        public string ?DealName { get; set; }

    
    public string? DealType { get; set; }

    
    public string? SourceListId { get; set; }

   
    public string? Side { get; set; }
}
