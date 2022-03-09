using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundStack.Data.Models
{
    public class Portfolio
    {
        [Key, ForeignKey("User")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

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

        public User User { get; set; }

        public IEnumerable<Asset> Assets { get; set; } = new List<Asset>();
    }
}
