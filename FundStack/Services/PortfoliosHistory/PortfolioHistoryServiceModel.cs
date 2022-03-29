namespace FundStack.Services.PortfoliosHistory
{
    public class PortfolioHistoryServiceModel
    { 
        public string PortfolioId { get; set; }

        public decimal PortfolioValue { get; set; }

        public DateTime SnapshotDate { get; set; }
    }
}
