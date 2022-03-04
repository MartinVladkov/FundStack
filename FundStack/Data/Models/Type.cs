namespace FundStack.Data.Models
{
    public class Type
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Asset> Assets { get; set; } = new List<Asset>();
    }
}
