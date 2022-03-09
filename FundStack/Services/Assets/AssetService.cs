using FundStack.Data;
using System.Text.Json;

namespace FundStack.Services.Assets
{
    public class AssetService : IAssetService
    {
        private FundStackDbContext data { get; set; }

        public AssetService(FundStackDbContext data)
        {
            this.data = data;
        }

        public List<AllAssetServiceModel> All()
        {
            var assets = this.data
                .Assets
                .OrderByDescending(a => a.Id)
                .Select(a => new AllAssetServiceModel
                {
                    Id = a.Id,
                    Name = a.Name.ToUpper(),
                    Type = a.Type.Name,
                    BuyPrice = a.BuyPrice,
                    BuyDate = a.BuyDate,
                    InvestedMoney = a.InvestedMoney,
                    //CurrentPrice = GetCurrentPrice(a.Name),
                    Description = a.Description
                }).ToList();

            return assets;
        }

        private static decimal GetCurrentPrice(string assetName)
        {
            decimal currentPrice;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://alpha-vantage.p.rapidapi.com/query?function=GLOBAL_QUOTE&symbol=" + assetName + "&datatype=json"),
                Headers =
    {
        { "x-rapidapi-host", "alpha-vantage.p.rapidapi.com" },
        { "x-rapidapi-key", "35fe9782bemsh451fb4a3e35b09ap115869jsnaa66c6d112dd" },
    },
            };

            using (var response = client.Send(request))
            {
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStream();
                AssetJsonModel jsonObject = JsonSerializer.Deserialize<AssetJsonModel>(body);
                currentPrice = jsonObject.Price;
            }

            return 233;
        }

    }
}
