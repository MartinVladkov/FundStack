namespace FundStack.Data.Models
{
    public class Type
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Asset> Assets { get; set; } = new List<Asset>();

        public IEnumerable<AssetHistory> AssetsHistory { get; set; } = new List<AssetHistory>();
    }
}
