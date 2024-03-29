﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FundStack.Data.DataConstants;
namespace FundStack.Data.Models
{
    public class Asset
    {
        [Key]
        [Required]
        public int Id { get; set; } 

        [Required]
        [StringLength(AssetNameMaxLength, MinimumLength = AssetNameMinLength)]
        public string Name { get; set; }

        [Required]
        [Range(MinPrice, double.MaxValue)]
        [Column(TypeName = "decimal(18,10)")]
        public decimal BuyPrice { get; set; }

        public DateTime BuyDate { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InvestedMoney { get; set; }
      
        [Column(TypeName = "decimal(18,8)")]
        public decimal? Amount { get; set; } //calculated given buy price and invested amount

        [Column(TypeName = "decimal(18,10)")]
        public decimal? CurrentPrice { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public decimal? SellPrice { get; set; }

        public DateTime? SellDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProfitLoss { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProfitLossPercent { get; set; }

        [MaxLength(350)]
        public string? Description { get; set; }

        public int TypeId { get; set; }
        public Type Type { get; set; }

        public string PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
