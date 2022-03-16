using FundStack.Data;

namespace FundStack.Services.Portfolio
{
    public class PortfolioService : IPortfolioService
    {
        private FundStackDbContext data { get; set; }

        public PortfolioService(FundStackDbContext data)
        {
            this.data = data;
        }

        public ValuePortfolioServiceModel Details(string userId)
        {
            var portfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .Select(p => new ValuePortfolioServiceModel
                {
                    UserId = p.UserId,
                    TotalValue = p.TotalValue,
                    AvailableMoney = p.AvailableMoney,
                    InvestedMoney = p.InvestedMoney,
                    ProfitLoss = p.ProfitLoss,
                    ProfitLossPercent = p.ProfitLossPercent,
                })
                .FirstOrDefault();

            return portfolio;
        }
    }
}
