﻿using FundStack.Data;
using FundStack.Data.Models;
using FundStack.Models.Assets;
using FundStack.Services.Assets;
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var asset = new Asset
            {
                Name = input.Name,
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

        public IActionResult All()
        {
            var assets = this.assets.All();
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
