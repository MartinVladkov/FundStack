using FundStack.Models.Assets;
using FundStack.Services.Assets;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundStack.Test.Mocks
{
    public static class AssetServiceMock
    {
        public static IAssetService Instance
        {
            get
            {
                var assetServiceMock = new Mock<IAssetService>();

                //GetAssetTypes()
                assetServiceMock
                    .Setup(a => a.GetAssetTypes())
                    .Returns(new List<AssetTypeViewModel>
                    {
                        new AssetTypeViewModel { Id = 1, Name = "Crypto" },
                        new AssetTypeViewModel { Id = 2, Name = "Stock" }
                    });

                //CheckNullAssetPrice()
                assetServiceMock
                    .Setup(a => a.CheckNullAssetPrice("nonValid"))
                    .Returns(true);

                assetServiceMock
                    .Setup(a => a.CheckNullAssetPrice("valid"))
                    .Returns(false);

                //Details()
                assetServiceMock
                    .Setup(a => a.Details(1))
                    .Returns(new SellAssetServiceModel
                    {
                        PortfolioId = "valid"
                    });

                assetServiceMock
                  .Setup(a => a.Details(2))
                  .Returns(new SellAssetServiceModel
                  {
                      PortfolioId = "wrong"
                  });

                return assetServiceMock.Object;
            }
        }
    }
}
