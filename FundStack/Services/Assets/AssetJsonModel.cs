using Newtonsoft.Json;

namespace FundStack.Services.Assets
{
    public class AssetJsonModel
    {
        [JsonProperty(PropertyName = "01. symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "02. open")]
        public string Open { get; set; }

        [JsonProperty(PropertyName = "03. high")]
        public string High { get; set; }

        [JsonProperty(PropertyName = "04. low")]
        public string Low { get; set; }

        [JsonProperty(PropertyName = "05. price")]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "06. volume")]
        public string Volume { get; set; }

        [JsonProperty(PropertyName = "07. latest trading day")]
        public string LatestDay { get; set; }

        [JsonProperty(PropertyName = "08. previous close")]
        public string PrevClose { get; set; }

        [JsonProperty(PropertyName = "09. change")]
        public string Change { get; set; }

        [JsonProperty(PropertyName = "10. change percent")]
        public string ChangePercent { get; set; }
    }

    public class Root
    {
        [JsonProperty("Global Quote")]
        public AssetJsonModel GlobalQuote { get; set; }

        public Root(AssetJsonModel GlobalQuote)
        {
            this.GlobalQuote = GlobalQuote;
        }
    }
}
