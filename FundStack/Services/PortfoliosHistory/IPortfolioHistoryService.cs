namespace FundStack.Services.PortfoliosHistory
{
    public interface IPortfolioHistoryService
    {
        List<PortfolioHistoryServiceModel> TotalValue(string userId);

        void RecordPorfoltioValue();
    }
}
