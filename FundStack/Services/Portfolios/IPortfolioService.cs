using FundStack.Data.Models;

namespace FundStack.Services.Portfolios
{
    public interface IPortfolioService
    {
        void AddMoney(string userId, decimal addedMoney);

        void WithdrawMoney(string userId, decimal addedMoney);

        Portfolio GetCurrentPortfolio(string userId);

        ValuePortfolioServiceModel Details(string userId);
    }
}
