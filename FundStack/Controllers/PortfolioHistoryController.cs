using FundStack.Models.PortfolioHistory;
using FundStack.Services.PortfoliosHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundStack.Controllers
{
    public class PortfolioHistoryController : Controller 
    {
        private readonly IPortfolioHistoryService portfolioHistory;

        public PortfolioHistoryController(IPortfolioHistoryService portfolioHistory)
        {
            this.portfolioHistory = portfolioHistory;
        }

        [Authorize]
        public IActionResult TotalValue()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currPortfolioHistory = portfolioHistory.TotalValue(userId);

            if (currPortfolioHistory.Count == 0)
            {
                return View("NoFunds");
            }

            var portfolioView = new PortfolioHistoryViewModel();

            foreach (var item in currPortfolioHistory)
            {
                portfolioView.History.Add(item.SnapshotDate.ToShortDateString(), item.PortfolioValue);
            }

            return View(portfolioView);
        }
    }
}
