using FundStack.Data;
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
    }
}
