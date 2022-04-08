using FundStack.Data;
using FundStack.Data.Models;
using FundStack.Models.Assets;
using Microsoft.EntityFrameworkCore;
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

        public void AddAsset(string userId, AddAssetFormModel input)
        {
            var asset = new Asset
            {
                Name = input.Name.ToUpper(),
                TypeId = input.TypeId,
                BuyPrice = input.BuyPrice,
                InvestedMoney = input.InvestedMoney,
                Description = input.Description,
                BuyDate = DateTime.UtcNow,
                PortfolioId = userId
            };

            var currPortfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            currPortfolio.AvailableMoney -= input.InvestedMoney;

            this.data.Assets.Add(asset);
            this.data.SaveChanges();
        }

        public List<AllAssetServiceModel> All(string userId, int excludeRecords, int pageSize, string sortOrder)
        {
            var orderedAssets = this.data
                .Assets
                .Where(a => a.PortfolioId == userId)
                .Include(a => a.Type)
                .ToList();

            switch (sortOrder)
            {
                case "Name":
                    orderedAssets = orderedAssets.OrderBy(a => a.Name).ToList();
                    break;
                case "name_desc":
                    orderedAssets = orderedAssets.OrderByDescending(a => a.Name).ToList();
                    break;
                case "Type":
                    orderedAssets = orderedAssets.OrderBy(a => a.Type.Name).ToList();
                    break;
                case "type_desc":
                    orderedAssets = orderedAssets.OrderByDescending(a => a.Type.Name).ToList();
                    break;
                case "InvestedMoney":
                    orderedAssets = orderedAssets.OrderBy(a => a.InvestedMoney).ToList();
                    break;
                case "investedMoney_desc":
                    orderedAssets = orderedAssets.OrderByDescending(a => a.InvestedMoney).ToList();
                    break;
                case "ProfitLoss":
                    orderedAssets = orderedAssets.OrderBy(a => a.ProfitLoss).ToList();
                    break;
                case "profitLoss_desc":
                    orderedAssets = orderedAssets.OrderByDescending(a => a.ProfitLoss).ToList();
                    break;
                case "ProfitLossPercent":
                    orderedAssets = orderedAssets.OrderBy(a => a.ProfitLossPercent).ToList();
                    break;
                case "profitLossPercent_desc":
                    orderedAssets = orderedAssets.OrderByDescending(a => a.ProfitLossPercent).ToList();
                    break;
                case "date_desc":
                    orderedAssets = orderedAssets.OrderByDescending(a => a.BuyDate).ToList();
                    break;
                default:
                    orderedAssets = orderedAssets.OrderBy(a => a.BuyDate).ToList();
                    break;
            }
            
            var assets = orderedAssets
                .Skip(excludeRecords)
                .Take(pageSize)
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
                })
                .ToList();
            
            return assets;
        }

        public void UpdateDatabase()
        {
            List<string> cryptoAssetNames = this.data
                .Assets
                .Where(a => a.Type.Name == "Crypto")
                .GroupBy(a => a.Name)
                .Select(y => y.Key)
                .ToList();

            List<string> stockAssetNames = this.data
               .Assets
               .Where(a => a.Type.Name == "Stock" || a.Type.Name == "ETF")
               .GroupBy(a => a.Name)
               .Select(y => y.Key)
               .ToList();

            if (cryptoAssetNames.Count > 0)
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

            if (stockAssetNames.Count > 0)
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
                int i = 0;
                foreach (var coin in coinsJson)
                {
                    price = Decimal.Parse(coin.GetProperty("price").ToString());
                    assetSymbol = coin.GetProperty("symbol").ToString();
                    result.Add(assetSymbol, price);

                    i++;
                    if(i >= assetName.Count)
                    {
                        break;
                    }
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
                    assetSymbol = asset;
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

                asset.ProfitLossPercent = Math.Round((decimal)(difference / asset.BuyPrice * 100), 2);
                asset.ProfitLoss = Math.Round((decimal)(asset.InvestedMoney * asset.ProfitLossPercent / 100), 2);
                asset.Amount = Math.Round((decimal)(asset.InvestedMoney / asset.BuyPrice), 2);
            }
            this.data.SaveChanges();
        }

        public SellAssetServiceModel Details(int assetId)
        {
            var asset = this.data
                .Assets
                .Where(a => a.Id == assetId)
                .Select(a => new SellAssetServiceModel
                {
                    Id = a.Id,
                    PortfolioId = a.PortfolioId, 
                    Name = a.Name,
                    BuyPrice = a.BuyPrice,
                    InvestedMoney = a.InvestedMoney,
                    CurrentPrice = a.CurrentPrice,
                    ProfitLoss = a.ProfitLoss,
                    ProfitLossPercent = a.ProfitLossPercent
                })
                .FirstOrDefault();

            return asset;
        }

        public void Sell(SellAssetServiceModel asset, string userId)
        {
            var soldAsset = this.data
                .Assets
                .Where(a => a.Id == asset.Id)
                .Include(a => a.Portfolio)
                .FirstOrDefault();

            soldAsset.Portfolio.AvailableMoney += soldAsset.InvestedMoney + (decimal)soldAsset.ProfitLoss;

            var assetHistory = new AssetHistory
            {
                Name = soldAsset.Name.ToUpper(),
                TypeId = soldAsset.TypeId,
                BuyPrice = soldAsset.BuyPrice,
                BuyDate = soldAsset.BuyDate,
                InvestedMoney = soldAsset.InvestedMoney,
                Amount = (decimal)soldAsset.Amount,
                SellPrice = (decimal)soldAsset.CurrentPrice,
                SellDate = DateTime.UtcNow,
                ProfitLoss = (decimal)soldAsset.ProfitLoss,
                ProfitLossPercent = (decimal)soldAsset.ProfitLossPercent,
                Description = soldAsset.Description,
                PortfolioId = userId
            };

            this.data.Assets.Remove(soldAsset);
            this.data.AssetsHistory.Add(assetHistory);
            this.data.SaveChanges();
        }

        public void Delete(int id, string userId)
        {
            var assetToDelete = this.data
                .Assets
                .Where(a => a.PortfolioId == userId)
                .Where(a => a.Id == id)
                .FirstOrDefault();

            var portfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            portfolio.AvailableMoney += assetToDelete.InvestedMoney;
            this.data.Assets.Remove(assetToDelete);
            this.data.SaveChanges();
        }

        public IEnumerable<AssetTypeViewModel> GetAssetTypes()
        {
            var types = this.data
                .Types
                .Select(t => new AssetTypeViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList();

            return types;
        }

        public int GetCount(string userId)
        {
            return this.data.Assets
                .Where(a => a.PortfolioId == userId)
                .Count();
        }

        public bool CheckNullAssetPrice(string userId)
        {
            if(this.data.Assets.Where(a => a.PortfolioId == userId).Any(a => a.CurrentPrice == null))
            {
                return true;
            }
            return false;
        }
    }
}
