using FundStack.Data;
using FundStack.Models.Portfolio;
using FundStack.Services.Portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundStack.Controllers
{
    public class PortfolioController : Controller 
    {
        private readonly IPortfolioService portfolio;
        private readonly FundStackDbContext data;
        

        public PortfolioController(IPortfolioService portfolio, FundStackDbContext data)
        {
            this.portfolio = portfolio;
            this.data = data;
        }

        [Authorize]
        public IActionResult Value()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            if (!ModelState.IsValid)
            {
                return View();
            }

            var money = addedMoney.AvailableMoney;
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
            var money = withdrawMoney.AvailableMoney;
            var currPortfolio = this.data
                .Portfolios
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            if (currPortfolio.AvailableMoney < money)
            {
                ModelState.AddModelError("AvailableMoney", "Cannot withdraw more money than available in portfolio");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            portfolio.WithdrawMoney(userId, money);

            return RedirectToAction(nameof(Value));
        }
    }
}
