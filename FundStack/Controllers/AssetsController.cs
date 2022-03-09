using FundStack.Data;
using FundStack.Data.Models;
using FundStack.Models.Assets;
using Microsoft.AspNetCore.Mvc;

namespace FundStack.Controllers
{
    public class AssetsController : Controller 
    {
        private FundStackDbContext data { get; set; }

        public AssetsController(FundStackDbContext data)
        {
            this.data = data;
        }

        public IActionResult AddAsset()
        {
            return View(new AddAssetFormModel
            {
                Types = this.GetAssetTypes()
            });
        }

        [HttpPost]
        public IActionResult AddAsset(AddAssetFormModel input)
        {
            if (!this.data.Types.Any(t => t.Id == input.TypeId))
            {
                this.ModelState.AddModelError(nameof(input.TypeId), "Type is not valid");
            }

            if (!ModelState.IsValid)
            {
                input.Types = this.GetAssetTypes();

                return View(input);
            }

            var asset = new Asset
            {
                Name = input.Name,
                TypeId = input.TypeId,
                BuyPrice = input.BuyPrice,
                InvestedMoney = input.InvestedMoney,
                Description = input.Description,
                BuyDate = DateTime.UtcNow
            };
            
            this.data.Assets.Add(asset);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<AssetTypeViewModel> GetAssetTypes() 
            => data
                .Types
                .Select(t => new AssetTypeViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList();

        public IActionResult All()
        {
            var assets = this.data
                .Assets
                .OrderByDescending(a => a.Id)
                .Select(a => new AllAssetsViewModel
                {
                    Id = a.Id,
                    Name=a.Name.ToUpper(),
                    Type = a.Type.Name,
                    BuyPrice=a.BuyPrice,
                    BuyDate = a.BuyDate,
                    InvestedMoney=a.InvestedMoney,
                    Description = a.Description
                }).ToList();

            return View(assets);
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
