using ClosedXML.Excel;
using FundStack.Services.Assets;
using FundStack.Services.AssetsHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundStack.Controllers
{
    public class AssetsHistoryController : Controller 
    {
        private readonly IAssetHistoryService assetsHistory;

        public AssetsHistoryController(IAssetHistoryService assetsHistory)
        {
            this.assetsHistory = assetsHistory;
        }

        [Authorize]
        public IActionResult AllHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var assets = this.assetsHistory.AllHistory(userId);
            return View(assets);
        }

        [Authorize(Roles = "Admin, PremiumUser")]
        [HttpPost]
        public IActionResult Export()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var content = this.assetsHistory.Export(userId);
            return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Assets.xlsx");
        }
    }
}
