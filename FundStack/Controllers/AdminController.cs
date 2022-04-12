using FundStack.Data.Models;
using FundStack.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FundStack.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService admin;

        public AdminController(IAdminService admin)
        {
            this.admin = admin;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AllUsers()
        {
            var allUsers = admin.GetAllUsers();

            return View(allUsers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AllUsers(List<AllUsersServiceModel> users)
        {
            admin.ChangeRole(users);
            return RedirectToAction("FearAndGreedIndex", "Home", new { area = "" });
        }
    }
}
