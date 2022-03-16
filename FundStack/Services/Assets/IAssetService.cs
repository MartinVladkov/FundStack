namespace FundStack.Services.Assets
{
    public interface IAssetService
    {
        List<AllAssetServiceModel> All(string userId);

        SellAssetServiceModel Details(int assetId);

        void Sell(SellAssetServiceModel asset, string userId);

        void Delete(int assetId);
    }
}
