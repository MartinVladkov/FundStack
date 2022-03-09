using FundStack.Models.Assets;
using System.ComponentModel.DataAnnotations;
using static FundStack.Data.DataConstants;
namespace FundStack.Services.Assets
{
    public class AddAssetServiceModel
    {
        [Required]
        [StringLength(AssetNameMaxLength, MinimumLength = AssetNameMinLength, ErrorMessage = "The name of the asset must be between 2 and 15 characters")]
        public string Name { get; init; }

        [Range(MinPrice, double.MaxValue, ErrorMessage = "The buy price cannot be 0")]
        public decimal BuyPrice { get; init; }

        [Range(1, double.MaxValue, ErrorMessage = "Invested money must be more than 1")]
        public decimal InvestedMoney { get; init; }

        [MaxLength(350, ErrorMessage = "The maximum length of the description is 350 characters")]
        public string? Description { get; init; }

        public int TypeId { get; init; }

        public IEnumerable<AssetTypeViewModel>? Types { get; set; }
    }
}
