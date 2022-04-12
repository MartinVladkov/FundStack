using FundStack.Data;
using FundStack.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace FundStack.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly FundStackDbContext data;
        private readonly UserManager<User> userManager;

        public AdminService(FundStackDbContext data, UserManager<User> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public List<AllUsersServiceModel> GetAllUsers()
        {
            var users = new List<AllUsersServiceModel>();

            foreach (var user in data.Users)
            {
                if (userManager.GetRolesAsync(user).Result.Any(r => r == "PremiumUser"))
                {
                    users.Add(new AllUsersServiceModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsPremiumUser = true
                    });
                }

                else
                {
                    users.Add(new AllUsersServiceModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsPremiumUser = false
                    });
                }
            }

            return users;
        }

        public void ChangeRole(List<AllUsersServiceModel> users)
        {
            var allUsers = data.Users;

            foreach (var user in users)
            {
                var currUser = allUsers.Where(u => u.Id == user.UserId).FirstOrDefault();

                if (user.IsPremiumUser && !userManager.GetRolesAsync(currUser).Result.Any(r => r == "PremiumUser"))
                {
                    userManager.AddToRoleAsync(currUser, "PremiumUser");

                    if(userManager.GetRolesAsync(currUser).Result.Any(r => r == "User"))
                    {
                        userManager.RemoveFromRoleAsync(currUser, "User");
                    }
                }

                else if (!user.IsPremiumUser && !userManager.GetRolesAsync(currUser).Result.Any(r => r == "User"))
                {
                    userManager.AddToRoleAsync(currUser, "User");

                    if(userManager.GetRolesAsync(currUser).Result.Any(r => r == "PremiumUser"))
                    {
                        userManager.RemoveFromRoleAsync(currUser, "PremiumUser");
                    }
                }
            }

            data.SaveChanges();
        }
    }
}
