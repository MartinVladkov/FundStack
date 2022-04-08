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



                return assetServiceMock.Object;
            }
        }
    }
}
