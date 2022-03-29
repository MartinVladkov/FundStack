using FundStack.Models.Assets;
using System.ComponentModel.DataAnnotations;

namespace FundStack.Services.Assets
{
    public class AllAssetServiceModel
    {
        public int Id { get; set; }

        public string Name { get; init; }

        public decimal BuyPrice { get; init; }

        [DataType(DataType.Date)]
        public DateTime BuyDate { get; set; }

        public decimal InvestedMoney { get; init; }

        public decimal? Amount { get; init; }

        public decimal? CurrentPrice { get; init; }

        public decimal? ProfitLoss { get; set; }

        public decimal? ProfitLossPercent { get; set; }

        public string? Description { get; init; }

        public string Type { get; set; }

        public IEnumerable<AssetTypeViewModel>? Types { get; set; }

        public string PortfolioId { get; init; }

        public int AssetsCount { get; set; }
    }
}
