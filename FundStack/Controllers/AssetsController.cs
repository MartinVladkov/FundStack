﻿using FundStack.Data;
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

            if (!ModelState.IsValid)
            {
                input.Types = this.GetAssetTypes();

                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
        [HttpGet]
        public IActionResult Sell(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var asset = assets.Details(id);

            if (!userId.Equals(asset.PortfolioId))
            {
                return Unauthorized();
            }

            //return Json(new { success = true, url = Url.Action("Sell", asset) });
            return View(asset);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Sell(SellAssetServiceModel asset)
        {
            Sell(asset);
            return RedirectToAction(nameof(All));
        }
    }
}
