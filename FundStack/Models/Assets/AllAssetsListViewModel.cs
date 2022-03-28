using FundStack.Services.Assets;

namespace FundStack.Models.Assets
{
    public class AllAssetsListViewModel
    {
        public List<AllAssetServiceModel> Assets { get; set; }

        public int PageNumber { get; set; }

        public int AssetsPerPage { get; set; }

        public int AssetsCount { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;

        public int PagesCount => (int)Math.Ceiling((double)this.AssetsCount / this.AssetsPerPage);
    }
}
