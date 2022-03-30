namespace FundStack.Services.AssetsHistory
{
    public interface IAssetHistoryService
    {
        List<AssetHistoryServiceModel> AllHistory(string userId);

        public byte[] Export(string userId);
    }
}
