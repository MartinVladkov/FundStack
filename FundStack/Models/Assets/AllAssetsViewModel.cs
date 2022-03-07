namespace FundStack.Models.Assets
{
    public class AllAssetsViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Type { get; set; }

        public decimal BuyPrice { get; set; }

        public DateTime BuyDate { get; set; }

        public decimal InvestedMoney { get; set; }

        public string Description { get; set; }
    }
}
