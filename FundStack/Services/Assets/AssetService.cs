using FundStack.Data;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text;
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
                    CurrentPrice = GetCurrentPrice(a.Name, a.Type.Name),
                    Description = a.Description
                }).ToList();

            return assets;
        }

        private static decimal GetCurrentPrice(string assetName, string assetType)
        {
            string connectionString;
            decimal price = 0;

            if(assetType == "Crypto")
            {
                connectionString = "https://coinranking1.p.rapidapi.com/coins?symbols=" + assetName;
                var response = GetApiResponse(assetName, connectionString);
                var doc = JsonDocument.Parse(response.Result);
                var coinsJson = doc.RootElement.GetProperty("data").GetProperty("coins").EnumerateArray();
                foreach (var coin in coinsJson)
                {
                    price = Decimal.Parse(coin.GetProperty("price").ToString());
                    break;
                }
            }
            else
            {
                connectionString = "https://twelve-data1.p.rapidapi.com/price?symbol="+ assetName + "&format=json&outputsize=30";
                var response = GetApiResponse(assetName, connectionString);
                var doc = JsonDocument.Parse(response.Result);
                price = Decimal.Parse(doc.RootElement.GetProperty("price").ToString());
            }

            return price;
        }

        private static async Task<string> GetApiResponse(string assetName, string connectionString)
        {
            var client = new HttpClient();
   
            client.DefaultRequestHeaders.Add("x-rapidapi-key", "35fe9782bemsh451fb4a3e35b09ap115869jsnaa66c6d112dd");
            string response = await client.GetStringAsync(connectionString);
            
            Console.WriteLine(response);
            return response;
        }

    }
}
