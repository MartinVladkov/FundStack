using FundStack.Models.Users;

namespace FundStack.Services.Admin
{
    public interface IAdminService
    {
        List<AllUsersServiceModel> GetAllUsers();

        Task ChangeRole(List<ManageRolesViewModel> userRoles, string userId);

        Task<List<ManageRolesViewModel>> GetUserRoles(string userId);

        string GetUserName (string userId);
    }
}
