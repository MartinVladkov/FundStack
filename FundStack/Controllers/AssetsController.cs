using FundStack.Data;
using FundStack.Data.Models;
using FundStack.Models.Assets;
using FundStack.Services.Assets;
using FundStack.Services.Portfolios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace FundStack.Controllers
{
    public class AssetsController : Controller 
    {
        private readonly IAssetService assets;
        private readonly IPortfolioService portfolio;

        public AssetsController(IAssetService assets, IPortfolioService portfolio)
        {
            this.assets = assets;
            this.portfolio = portfolio;
        }

        [Authorize]
        public IActionResult AddAsset()
        {
            return View(new AddAssetFormModel
            {
                Types = assets.GetAssetTypes()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAsset(AddAssetFormModel input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currPortfolio = portfolio.GetCurrentPortfolio(userId);
            var types = assets.GetAssetTypes();

            if (!types.Any(t => t.Id == input.TypeId))
            {
                this.ModelState.AddModelError(nameof(input.TypeId), "Type is not valid");
            }

            if (currPortfolio.AvailableMoney < input.InvestedMoney)
            {
                ModelState.AddModelError("InvestedMoney", "Not enough available money for this investment. You can add more from your portfolio page.");
            }

            if (!ModelState.IsValid)
            {
                input.Types = assets.GetAssetTypes();

                return View(input);
            }

            assets.AddAsset(userId, input);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult All(string sortOrder, int pageNumber = 1)
        {
            const int pageSize = 5;
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            DateTime timeNow = DateTime.UtcNow;
            var timeNowString = timeNow.ToString();
           
            var cookie = HttpContext.Request.Cookies["timer"];

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var priceCheck = assets.CheckNullAssetPrice(userId);

            if (cookie == null)
            {
                HttpContext.Response.Cookies.Append("timer", timeNowString);
                
                this.assets.UpdateDatabase();
            }
            else
            {
                if(DateTime.Parse(cookie.ToString()).AddMinutes(10) < DateTime.UtcNow || priceCheck)
                {
                    this.assets.UpdateDatabase();
                    HttpContext.Response.Cookies.Append("timer", timeNowString);
                }
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";
            ViewData["BuyDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["InvestedMoneySortParm"] = sortOrder == "InvestedMoney" ? "investedMoney_desc" : "InvestedMoney";
            ViewData["ProfitLossSortParm"] = sortOrder == "ProfitLoss" ? "profitLoss_desc" : "ProfitLoss";
            ViewData["ProfitLossPercentSortParm"] = sortOrder == "ProfitLossPercent" ? "profitLossPercent_desc" : "ProfitLossPercent";

            var selectedAssets = this.assets.All(userId, excludeRecords, pageSize, sortOrder);

            var viewModel = new AllAssetsListViewModel
            {
                Assets = selectedAssets,
                AssetsCount = this.assets.GetCount(userId),
                PageNumber = pageNumber,
                AssetsPerPage = pageSize
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult All(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            assets.Delete(id, userId);
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
