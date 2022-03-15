namespace FundStack.Services.Assets
{
    public class SellAssetServiceModel
    {
        public int Id { get; set; }

        public string PortfolioId { get; set; }

        public string Name { get; init; }

        public decimal BuyPrice { get; init; }

        public decimal InvestedMoney { get; set; }

        public decimal? CurrentPrice { get; init; }

        public decimal? ProfitLoss { get; set; }

        public decimal? ProfitLossPercent { get; set; }

        public decimal? SellPrice { get; set; }
    }
}
