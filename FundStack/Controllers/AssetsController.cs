using FundStack.Data;
using FundStack.Data.Models;
using FundStack.Models.Assets;
using FundStack.Services.Assets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundStack.Controllers
{
    public class AssetsController : Controller 
    {
        private readonly IAssetService assets;
        private readonly FundStackDbContext data;

        public AssetsController(IAssetService assets, FundStackDbContext data)
        {
            this.assets = assets;  
            this.data = data;
        }

        [Authorize]
        public IActionResult AddAsset()
        {
            return View(new AddAssetFormModel
            {
                Types = this.GetAssetTypes()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAsset(AddAssetFormModel input)
        {
            if (!this.data.Types.Any(t => t.Id == input.TypeId))
            {
                this.ModelState.AddModelError(nameof(input.TypeId), "Type is not valid");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currPortfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            if (currPortfolio.AvailableMoney < input.InvestedMoney)
            {
                ModelState.AddModelError("InvestedMoney", "Not enough available money for this investment. You can add more from your portfolio page.");
            }

            if (!ModelState.IsValid)
            {
                input.Types = this.GetAssetTypes();

                return View(input);
            }
            
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

            currPortfolio.AvailableMoney -= input.InvestedMoney;

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

        [Authorize]
        public IActionResult All()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var assets = this.assets.All(userId);
            return View(assets);
        }

        [Authorize]
        [HttpPost]
        public IActionResult All(int id)
        {
            assets.Delete(id);
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Sell(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var asset = assets.Details(id);

            if (!userId.Equals(asset.PortfolioId))
            {
                return Unauthorized();
            }

            return View(asset);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Sell(SellAssetServiceModel asset)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            assets.Sell(asset, userId);
            return RedirectToAction(nameof(All));
        }
    }
}
