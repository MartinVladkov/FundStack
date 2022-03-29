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

            var portfolioView = portfolioHistory.TotalValue(userId);

            return View(portfolioView);
        }
    }
}
