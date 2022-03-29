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
    }
}
