using FundStack.Data;
using FundStack.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FundStack.Services.Portfolios
{
    public class PortfolioService : IPortfolioService
    {
        private FundStackDbContext data { get; set; }

        public PortfolioService(FundStackDbContext data)
        {
            this.data = data;
        }

        
        public void AddMoney(string userId, decimal addedMoney)
        {
            var portfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            portfolio.AvailableMoney += addedMoney;
            this.data.SaveChanges();
        }

        public void WithdrawMoney(string userId, decimal withdrawnMoney)
        {
            var portfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            portfolio.AvailableMoney -= withdrawnMoney;
            this.data.SaveChanges();
        }

        public ValuePortfolioServiceModel Details(string userId)
        {
            CalculatePortfolioValue(userId);

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

        public Portfolio GetCurrentPortfolio(string userId)
        {
            var currPortfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            return currPortfolio;
        }

        private void CalculatePortfolioValue(string userId)
        {
            var portfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .Include(p => p.Assets)
                .FirstOrDefault();

            portfolio.InvestedMoney = portfolio.Assets
                .ToList()
                .Sum(a => a.InvestedMoney);

            portfolio.ProfitLoss = (decimal)portfolio.Assets
                .ToList()
                .Sum(a => a.ProfitLoss);

            portfolio.ProfitLossPercent = (portfolio.ProfitLoss / (portfolio.InvestedMoney + portfolio.AvailableMoney)) * 100;

            portfolio.TotalValue = portfolio.AvailableMoney + portfolio.InvestedMoney + portfolio.ProfitLoss;

            this.data.SaveChanges();
        }

        public PortfolioStatisticServiceModel GetPortfolioStats(string userId)
        {
            var portfolioAssets = this.data
            .Assets
            .Where(a => a.PortfolioId == userId)
            .Include(a => a.Type)
            .ToList();

            //Dictionary<string, Dictionary<string, decimal>> groupedAssets = portfolioAssets
            //   .GroupBy(a => a.Type.Name)
            //   .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Name.ToUpper()).ToDictionary(y => y.Key, y => y.Sum(y => y.InvestedMoney)));

            Dictionary<string, decimal> cryptoStats = portfolioAssets.Where(a => a.Type.Name == "Crypto")
                .GroupBy(y => y.Name.ToUpper())
                .ToDictionary(y => y.Key, y => y.Sum(y => y.InvestedMoney));

            Dictionary<string, decimal> stockStats = portfolioAssets.Where(a => a.Type.Name == "Stock")
                .GroupBy(y => y.Name.ToUpper())
                .ToDictionary(y => y.Key, y => y.Sum(y => y.InvestedMoney));

            Dictionary<string, decimal> etfStats = portfolioAssets.Where(a => a.Type.Name == "ETF")
                .GroupBy(y => y.Name.ToUpper())
                .ToDictionary(y => y.Key, y => y.Sum(y => y.InvestedMoney));

            Dictionary<string, decimal> assetTypeValue = portfolioAssets
                .GroupBy(a => a.Type.Name)
                .ToDictionary(x => x.Key, x => x.Sum(x => x.InvestedMoney));

            var portfolioStats = new PortfolioStatisticServiceModel();
            portfolioStats.CryptoStatistics = cryptoStats;
            portfolioStats.StockStatistics = stockStats;
            portfolioStats.EtfStatistics = etfStats;
            portfolioStats.TotalStatistics = assetTypeValue;
            
            return portfolioStats;
        }
    }
}
