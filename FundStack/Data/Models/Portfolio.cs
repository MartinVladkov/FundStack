using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundStack.Data.Models
{
    public class Portfolio
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InitialInvestment { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableMoney { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InvestedMoney { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProfitLoss { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProfitLossPercent { get; set; }

        //public IEnumerable<Asset> Assets { get; set; } = new List<Asset>();// ().Where(a => a.Type == AssetType.CryptoCurrency);

        //public IEnumerable<Asset> Stocks { get; set; } = new List<Asset>().Where(a => a.Type == AssetType.Stock);

        //prop User User{}
    }
}
