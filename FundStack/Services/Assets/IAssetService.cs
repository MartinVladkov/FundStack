using FundStack.Models.Assets;

namespace FundStack.Services.Assets
{
    public interface IAssetService
    {
        void AddAsset(string userId, AddAssetFormModel input);

        List<AllAssetServiceModel> All(string userId, int excludeRecords, int pageSize, string sortOrder);

        void UpdateDatabase();

        int GetCount(string userId);

        IEnumerable<AssetTypeViewModel> GetAssetTypes();

        SellAssetServiceModel Details(int assetId);

        void Sell(SellAssetServiceModel asset, string userId);

        void Delete(int assetId, string userId);

        bool CheckNullAssetPrice(string userId);
    }
}
