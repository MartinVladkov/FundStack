namespace FundStack.Services.Portfolios
{
    public class PortfolioStatisticServiceModel
    {
        public Dictionary<string, decimal> CryptoStatistics { get; set; } = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> StockStatistics { get; set; } = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> EtfStatistics { get; set; } = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> TotalStatistics { get; set; } = new Dictionary<string, decimal>();
    }
}
