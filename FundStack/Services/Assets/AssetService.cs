using FundStack.Data;
using Newtonsoft.Json;
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
                    CurrentPrice = CurrPrice(a.Name),
                    Description = a.Description
                }).ToList();

            return assets;
        }

        private static decimal CurrPrice(string assetName)
        {
            var stream = GetCurrentPrice(assetName);
            string? fileContents;
            //using (StreamReader reader = new StreamReader(stream.Result))
            //{
            //    fileContents = reader.ReadToEnd();
            //}
            //byte[] byteArray = Encoding.UTF8.GetBytes(fileContents);
            //MemoryStream memoryStream = new MemoryStream(byteArray);
            var jsonObject = JsonConvert.DeserializeObject<Root>(stream.Result);
            //AssetJsonModel jsonObject = JsonSerializer.Deserialize<AssetJsonModel>(stream.RE);
            AssetJsonModel obj = jsonObject.GlobalQuote;
            decimal currentPrice = decimal.Parse(obj.Price);
            return currentPrice;
        }

        private static async Task<string> GetCurrentPrice(string assetName)
        {
            //decimal currentPrice;
            var client = new HttpClient();
    //        var request = new HttpRequestMessage
    //        {
    //            Method = HttpMethod.Get,
    //            RequestUri = new Uri("https://alpha-vantage.p.rapidapi.com/query?function=GLOBAL_QUOTE&symbol=" + assetName + "&datatype=json"),
    //            Headers =
    //{
    //    { "x-rapidapi-host", "alpha-vantage.p.rapidapi.com" },
    //    { "x-rapidapi-key", "35fe9782bemsh451fb4a3e35b09ap115869jsnaa66c6d112dd" },
    //},
    //        };
            client.DefaultRequestHeaders.Add("x-rapidapi-key", "35fe9782bemsh451fb4a3e35b09ap115869jsnaa66c6d112dd");
            string response = await client.GetStringAsync("https://alpha-vantage.p.rapidapi.com/query?function=GLOBAL_QUOTE&symbol=" + assetName + "&datatype=json");
            //response.EnsureSuccessStatusCode();
            //AssetJsonModel? jsonObject = response.Content.ReadFromJsonAsync<AssetJsonModel>().Result;
            //var body = response.Content;
            //AssetJsonModel? jsonObject = JsonSerializer.Deserialize<AssetJsonModel>(response);
            //currentPrice = jsonObject.Price;

            //var client = new RestClient("https://alpha-vantage.p.rapidapi.com/");
            //var request = new RestRequest($"query?function=GLOBAL_QUOTE&symbol={assetName}&datatype=json", Method.Get);
            //request.AddHeader("x-rapidapi-host", "alpha-vantage.p.rapidapi.com");
            //request.AddHeader("x-rapidapi-key", "35fe9782bemsh451fb4a3e35b09ap115869jsnaa66c6d112dd");
            //RestResponse response = client.GetAsync(request).Wait();
            //var response = client.GetAsync<AssetJsonModel>(request);
            //AssetJsonModel jsonObject = JsonSerializer.Deserialize<AssetJsonModel>(response);
            //Console.WriteLine(response.Co);
            //AssetJsonModel? jsonObject = JsonSerializer.Deserialize<AssetJsonModel>(response);
            //decimal currentPrice = response.Price;
            Console.WriteLine(response);
            return response;
        }

    }
}
