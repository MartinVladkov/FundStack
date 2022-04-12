namespace FundStack.Services.Admin
{
    public interface IAdminService
    {
        List<AllUsersServiceModel> GetAllUsers();

        void ChangeRole(List<AllUsersServiceModel> users);
    }
}
