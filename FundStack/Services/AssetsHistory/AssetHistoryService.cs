using ClosedXML.Excel;
using FundStack.Data;
using System.Data;

namespace FundStack.Services.AssetsHistory
{
    public class AssetHistoryService : IAssetHistoryService
    {
        private FundStackDbContext data { get; set; }

        public AssetHistoryService(FundStackDbContext data)
        {
            this.data = data;
        }

        public List<AssetHistoryServiceModel> AllHistory(string userId)
        {
            var assets = this.data
                .AssetsHistory
                .Where(a => a.PortfolioId == userId)
                .OrderByDescending(a => a.SellDate)
                .Select(a => new AssetHistoryServiceModel
                {
                    Id = a.Id,
                    Name = a.Name.ToUpper(),
                    Type = a.Type.Name,
                    BuyPrice = a.BuyPrice,
                    BuyDate = a.BuyDate.Date,
                    InvestedMoney = a.InvestedMoney,
                    Amount = a.Amount,
                    SellPrice = a.SellPrice,
                    SellDate = a.SellDate,
                    ProfitLoss = a.ProfitLoss,
                    ProfitLossPercent = a.ProfitLossPercent,
                    Description = a.Description
                }).ToList();

            return assets;
        }

        public byte[] Export(string userId)
        {
            DataTable dt = new DataTable("Assets");
            dt.Columns.AddRange(new DataColumn[12]
            { new DataColumn("Id"),
              new DataColumn("Name"),
              new DataColumn("Type"),
              new DataColumn("BuyPrice"),
              new DataColumn("BuyDate"),
              new DataColumn("InvestedMoney"),
              new DataColumn("Amount"),
              new DataColumn("SellPrice"),
              new DataColumn("SellDate"),
              new DataColumn("ProfitLoss"),
              new DataColumn("ProfitLossPercent"),
              new DataColumn("Description")
            });

            var assets = AllHistory(userId);

            foreach (var asset in assets)
            {
                dt.Rows.Add(asset.Id, asset.Name, asset.Type, asset.BuyPrice,
                            asset.BuyDate, asset.InvestedMoney, asset.Amount, asset.SellPrice,
                            asset.SellDate, asset.ProfitLoss, asset.ProfitLossPercent, asset.Description);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }
    }
}
