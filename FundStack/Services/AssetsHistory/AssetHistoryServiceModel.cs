namespace FundStack.Services.AssetsHistory
{
    public class AssetHistoryServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal BuyPrice { get; set; }

        public DateTime BuyDate { get; set; }

        public decimal InvestedMoney { get; set; }

        public decimal Amount { get; set; } 

        public decimal SellPrice { get; set; }

        public DateTime SellDate { get; set; }

        public decimal ProfitLoss { get; set; }

        public decimal ProfitLossPercent { get; set; }

        public string Type { get; set; }

        public string? Description { get; set; }
    }
}
