using FundStack.Models.Portfolio;
using FundStack.Services.Portfolios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundStack.Controllers
{
    public class PortfolioController : Controller 
    {
        private readonly IPortfolioService portfolio;

        public PortfolioController(IPortfolioService portfolio)
        {
            this.portfolio = portfolio;
        }

        [Authorize]
        public IActionResult Value()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currPortfolio = this.portfolio.GetCurrentPortfolio(userId);
           
            if(currPortfolio.AvailableMoney == 0)
            {
                return View("NoFunds");
            }

            var userPorfolio = portfolio.Details(userId);
            return View(userPorfolio);
        }

        [Authorize]
        public IActionResult AddMoney()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddMoney(AddMoneyViewModel addedMoney)
        {
            if (addedMoney.Money < 1)
            {
                ModelState.AddModelError("AvailableMoney", "The added money cannot be less than $1");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var money = addedMoney.Money;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            portfolio.AddMoney(userId, money);

            return RedirectToAction(nameof(Value));
        }

        [Authorize]
        public IActionResult WithdrawMoney()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult WithdrawMoney(AddMoneyViewModel withdrawMoney)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var money = withdrawMoney.Money;
            var currPortfolio = this.portfolio.GetCurrentPortfolio(userId);

            if (currPortfolio.AvailableMoney < money)
            {
                ModelState.AddModelError("AvailableMoney", "Cannot withdraw more money than available in portfolio");
            }

            if (withdrawMoney.Money < 1)
            {
                ModelState.AddModelError("AvailableMoney", "Cannot withdraw less than $1");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            portfolio.WithdrawMoney(userId, money);

            return RedirectToAction(nameof(Value));
        }

        [Authorize]
        public IActionResult Statistics()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currPortfolio = this.portfolio.GetCurrentPortfolio(userId);

            if (currPortfolio.Assets.Count() == 0)
            {
                return View("NoAssets");
            }

            var portfolioStats = this.portfolio.GetPortfolioStats(userId);
            return View(portfolioStats);
        }
    }
}
