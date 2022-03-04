using FundStack.Data;
using FundStack.Data.Models;
using FundStack.Models.Assets;
using Microsoft.AspNetCore.Mvc;

namespace FundStack.Controllers
{
    public class AssetsController : Controller 
    {
        public FundStackDbContext data { get; set; }

        public AssetsController(FundStackDbContext data)
        {
            this.data = data;
        }

        public IActionResult AddAsset()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAsset(AddAssetFormModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var asset = new Asset
            {
                Name = input.Name,
                Type = (AssetType)input.Type,
                BuyPrice = input.BuyPrice,
                InvestedMoney = input.InvestedMoney,
                Description = input.Description,
                BuyDate = DateTime.UtcNow
            };
            
            this.data.Assets.Add(asset);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult SellAsset()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SellAsset(SellAssetFormModel asset)
        {
            return View();
        }
    }
}
