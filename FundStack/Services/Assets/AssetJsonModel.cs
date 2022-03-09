using Newtonsoft.Json;

namespace FundStack.Services.Assets
{
    public class AssetJsonModel
    {
        [JsonProperty(PropertyName = "01. symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "02. open")]
        public decimal Open { get; set; }

        [JsonProperty(PropertyName = "03. high")]
        public decimal High { get; set; }

        [JsonProperty(PropertyName = "04. low")]
        public decimal Low { get; set; }

        [JsonProperty(PropertyName = "05. price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "06. volume")]
        public decimal Volume { get; set; }

        [JsonProperty(PropertyName = "07. latest trading day")]
        public decimal LatestDay { get; set; }

        [JsonProperty(PropertyName = "08. previous close")]
        public decimal PrevClose { get; set; }

        [JsonProperty(PropertyName = "09. change")]
        public decimal Change { get; set; }

        [JsonProperty(PropertyName = "10. change percent")]
        public decimal ChangePercent { get; set; }
    }
}
