﻿namespace FundStack.Services.Portfolio
{
    public interface IPortfolioService
    {
        void AddMoney(string userId, decimal addedMoney);

        ValuePortfolioServiceModel Details(string userId);
    }
}
