namespace FundStack.Services.Assets
{
    public interface IAssetService
    {
        List<AllAssetServiceModel> All(string userId);

        SellAssetServiceModel Details(int assetId);
    }
}
