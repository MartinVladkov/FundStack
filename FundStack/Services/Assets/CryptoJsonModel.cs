using Newtonsoft.Json;

namespace FundStack.Services.Assets
{
    public class CryptoJsonModel
    {
        
        public string uuid { get; set; }

        public string symbol { get; set; }

        public string name { get; set; }

        public string color { get; set; }

        public string iconUrl { get; set; }

        public string marketCap { get; set; }


        [JsonProperty(PropertyName = "price")]
        public string CurrentPrice { get; set; }

        public int listedAt { get; set; }

        public int tier { get; set; }

        public string change { get; set; }

        public int rank { get; set; }

        public List<string> sparkline { get; set; }

        public bool lowVolume { get; set; }

        public string coinrankingUrl { get; set; }

        public string _24hVolume { get; set; }

        public string btcPrice { get; set; }
    }
}
