using FundStack.Data.Models;
using FundStack.Models.Users;
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
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var userRoles = await admin.GetUserRoles(userId);
            var userName = admin.GetUserName(userId);

            ViewBag.UserName = userName;
            ViewBag.UserId = userId;
            
            return View(userRoles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ManageRoles(List<ManageRolesViewModel> userRoles, string userId)
        {
            await admin.ChangeRole(userRoles, userId);

            return RedirectToAction(nameof(AllUsers));
        }
    }
}
