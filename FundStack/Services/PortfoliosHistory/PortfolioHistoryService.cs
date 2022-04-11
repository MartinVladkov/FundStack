using FundStack.Data;
using FundStack.Data.Models;

namespace FundStack.Services.PortfoliosHistory
{
    public class PortfolioHistoryService : IPortfolioHistoryService
    {
        private FundStackDbContext data { get; set; }

        public PortfolioHistoryService(FundStackDbContext data)
        {
            this.data = data;
        }

        public List<PortfolioHistoryServiceModel> TotalValue(string userId)
        {
            var portfolioHistory = this.data
                .PortfoliosHistory
                .Where(p => p.PortfolioId == userId)
                .OrderBy(p => p.SnapshotDate)
                .Select(p => new PortfolioHistoryServiceModel
                {
                    PortfolioId = p.PortfolioId,
                    PortfolioValue = p.PortfolioValue,
                    SnapshotDate = p.SnapshotDate
                })
                .ToList();

            return portfolioHistory;
        }

        public void RecordPorfoltioValue()
        {
            if(!this.data.PortfoliosHistory.Any(p => p.SnapshotDate == DateTime.UtcNow.Date))
            {
                foreach (var portfolio in data.Portfolios)
                {
                    var portfolioSnapshot = new PortfolioHistory
                    {
                        PortfolioId = portfolio.UserId,
                        PortfolioValue = portfolio.TotalValue,
                        SnapshotDate = DateTime.UtcNow.Date,
                    };
                    this.data.PortfoliosHistory.Add(portfolioSnapshot);
                }
                this.data.SaveChanges();
            }
            
        }   
    }
}
