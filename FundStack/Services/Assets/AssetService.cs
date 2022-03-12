using FundStack.Data;
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
            List<string> cryptoAssetNames = this.data
                .Assets
                .Where(a => a.Type.Name == "Crypto")
                .GroupBy(a => a.Name)
                .Select(y => y.Key)
                .ToList();

            List<string> stockAssetNames = this.data
               .Assets
               .Where(a => a.Type.Name == "Stock")
               .GroupBy(a => a.Name)
               .Select(y => y.Key)
               .ToList(); 

            if(cryptoAssetNames.Count > 0)
            {
                Dictionary<string, decimal> cryptoPrices = GetCurrentPrice(cryptoAssetNames, "Crypto");

                foreach (var group in cryptoPrices)
                {
                    foreach (var asset in data.Assets.Where(a => a.Name == group.Key))
                    {
                        asset.CurrentPrice = group.Value;
                    }
                }

                this.data.SaveChanges();
            }

            if(stockAssetNames.Count > 0)
            {
                Dictionary<string, decimal> stockPrices = GetCurrentPrice(stockAssetNames, "Stock");

                foreach (var group in stockPrices)
                {
                    foreach (var asset in data.Assets.Where(a => a.Name == group.Key))
                    {
                        asset.CurrentPrice = group.Value;
                    }
                }

                this.data.SaveChanges();
            }

            CalculateProfitLoss();

            var assets = this.data
                .Assets
                .OrderByDescending(a => a.Id)
                .Select(a => new AllAssetServiceModel
                {
                    Id = a.Id,
                    Name = a.Name.ToUpper(),
                    Type = a.Type.Name,
                    BuyPrice = a.BuyPrice,
                    BuyDate = a.BuyDate.Date,
                    InvestedMoney = a.InvestedMoney,
                    Amount = a.Amount,
                    CurrentPrice = a.CurrentPrice,
                    ProfitLoss = a.ProfitLoss,
                    ProfitLossPercent = a.ProfitLossPercent,
                    Description = a.Description
                }).ToList();

            return assets;
        }

        private static Dictionary<string, decimal> GetCurrentPrice(List<string> assetName, string assetType)
        {
            string connectionString;
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            decimal price = 0;
            string assetSymbol = "";

            if(assetType == "Crypto")
            {
                StringBuilder sb = new StringBuilder();

                foreach (var asset in assetName)
                {
                    sb.Append($"symbols={asset}&");
                }

                connectionString = "https://coinranking1.p.rapidapi.com/coins?" + sb.ToString();
                var response = GetApiResponse(connectionString);
                var doc = JsonDocument.Parse(response.Result);
                var coinsJson = doc.RootElement.GetProperty("data").GetProperty("coins").EnumerateArray();

                foreach (var coin in coinsJson)
                {
                    price = Decimal.Parse(coin.GetProperty("price").ToString());
                    assetSymbol = coin.GetProperty("symbol").ToString();
                    result.Add(assetSymbol, price);
                }
            }
            else
            {
                foreach (var asset in assetName)
                {
                    connectionString = "https://twelve-data1.p.rapidapi.com/price?symbol=" + asset + "&format=json&outputsize=30";
                    var response = GetApiResponse(connectionString);
                    var doc = JsonDocument.Parse(response.Result);
                    price = Decimal.Parse(doc.RootElement.GetProperty("price").ToString());
                    assetSymbol = assetName.FirstOrDefault();
                    result.Add(assetSymbol, price);
                }
            }

            return result;
        }

        private static async Task<string> GetApiResponse(string connectionString)
        {
            var client = new HttpClient();
   
            client.DefaultRequestHeaders.Add("x-rapidapi-key", "35fe9782bemsh451fb4a3e35b09ap115869jsnaa66c6d112dd");
            string response = await client.GetStringAsync(connectionString);
            
            Console.WriteLine(response);
            return response;
        }

        private void CalculateProfitLoss()
        {
            foreach (var asset in this.data.Assets)
            {
                var difference = asset.CurrentPrice - asset.BuyPrice;

                asset.ProfitLossPercent = difference / asset.BuyPrice * 100;
                asset.ProfitLoss = asset.InvestedMoney * asset.ProfitLossPercent / 100;
                asset.Amount = asset.InvestedMoney / asset.BuyPrice;
            }
            this.data.SaveChanges();
        }
    }
}
