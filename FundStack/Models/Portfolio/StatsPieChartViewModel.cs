namespace FundStack.Models.Portfolio
{
    public class StatsPieChartViewModel
    {
        public string Id { get; set; }

        public Dictionary<string, decimal> Statistics { get; set; } = new Dictionary<string, decimal>();
    }
}
