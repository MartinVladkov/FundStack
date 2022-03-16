namespace FundStack.Services.Portfolio
{
    public class ValuePortfolioServiceModel
    {
        public string UserId { get; set; }

        public decimal TotalValue { get; set; }

        public decimal AvailableMoney { get; set; }

        public decimal InvestedMoney { get; set; }

        public decimal ProfitLoss { get; set; }

        public decimal ProfitLossPercent { get; set; }
    }
}
