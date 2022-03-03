using FundStack.Data;
namespace FundStack.Models.Assets
{
    public class AddAssetFormModel
    {
        
        public string Name { get; init; }

        public int Type { get; init; }

        public decimal BuyPrice { get; init; }

        public decimal InvestedMoney { get; init; }

        public string? Description { get; init; }
    }
}
