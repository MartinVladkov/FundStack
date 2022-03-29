namespace FundStack.Models.PortfolioHistory
{
    public class PortfolioHistoryViewModel
    {
        public Dictionary<string, decimal> History { get; set; } = new Dictionary<string, decimal>();
    }
}
