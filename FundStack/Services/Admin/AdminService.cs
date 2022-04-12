using FundStack.Data;
using FundStack.Data.Models;
using FundStack.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace FundStack.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly FundStackDbContext data;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminService(FundStackDbContext data, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.data = data;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public List<AllUsersServiceModel> GetAllUsers()
        {
            var users = this.data.Users
                .Select(u => new AllUsersServiceModel
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToList();

            return users;
        }

        public async Task<List<ManageRolesViewModel>> GetUserRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            var userRoles = new List<ManageRolesViewModel>();
            
            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new ManageRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                userRoles.Add(userRolesViewModel);
            }

            return userRoles;
        }

        public async Task ChangeRole(List<ManageRolesViewModel> userRoles, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, roles);

            await userManager.AddToRolesAsync(user, userRoles.Where(x => x.Selected).Select(y => y.RoleName));
            data.SaveChanges();
        }

        public string GetUserName (string userId)
        {
            var userName = data.Users.Where(u => u.Id == userId)
                .Select(u => u.UserName)
                .FirstOrDefault();

            return userName;
        }
    }
}
