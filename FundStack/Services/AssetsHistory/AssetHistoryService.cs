using FundStack.Data;

namespace FundStack.Services.AssetsHistory
{
    public class AssetHistoryService : IAssetHistoryService
    {
        private FundStackDbContext data { get; set; }

        public AssetHistoryService(FundStackDbContext data)
        {
            this.data = data;
        }

        public List<AssetHistoryServiceModel> AllHistory(string userId)
        {
            var assets = this.data
                .AssetsHistory
                .Where(a => a.PortfolioId == userId)
                .OrderByDescending(a => a.Id)
                .Select(a => new AssetHistoryServiceModel
                {
                    Id = a.Id,
                    Name = a.Name.ToUpper(),
                    Type = a.Type.Name,
                    BuyPrice = a.BuyPrice,
                    BuyDate = a.BuyDate.Date,
                    InvestedMoney = a.InvestedMoney,
                    Amount = a.Amount,
                    SellPrice = a.SellPrice,
                    SellDate = a.SellDate,
                    ProfitLoss = a.ProfitLoss,
                    ProfitLossPercent = a.ProfitLossPercent,
                    Description = a.Description
                }).ToList();

            return assets;
        }
    }
}
