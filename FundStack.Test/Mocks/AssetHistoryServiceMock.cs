using FundStack.Services.AssetsHistory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundStack.Test.Mocks
{
    public static class AssetHistoryServiceMock
    {
        public static IAssetHistoryService Instance
        {
            get
            {
                var assetHistoryServiceMock = new Mock<IAssetHistoryService>();

                assetHistoryServiceMock
                    .Setup(a => a.AllHistory("valid"))
                    .Returns(new List<AssetHistoryServiceModel>
                    {
                        new AssetHistoryServiceModel { Id = 1, Name = "test1" },
                        new AssetHistoryServiceModel { Id = 2, Name = "test2" }
                    });

                return assetHistoryServiceMock.Object;
            }
        }
    }
}
