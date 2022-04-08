using FundStack.Controllers;
using FundStack.Models.Assets;
using FundStack.Services.Assets;
using FundStack.Test.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FundStack.Test.Controllers
{
    public class AssetsControllerTest
    {
        //Helper Classes
        private AssetsController ArrangeTest(string userId)
        {
            var assetService = AssetServiceMock.Instance;
            var portfolioService = PortfolioServiceMock.Instance;
            var assetsController = new AssetsController(assetService, portfolioService);

            assetsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = MockUser(userId) }
            };

            return assetsController;
        }

        private ClaimsPrincipal MockUser(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                   new Claim(ClaimTypes.Name, "example name"),
                   new Claim(ClaimTypes.NameIdentifier, userId),
                   new Claim("custom-claim", "example claim value"),
            }, "mock"));

            return user;
        }

        public static TheoryData<AddAssetFormModel> CorrectInputData()
        {
            return new TheoryData<AddAssetFormModel>
            {
                new AddAssetFormModel {Name = "1", BuyPrice = 1, InvestedMoney = 5, TypeId = 1},
                new AddAssetFormModel {Name = "2", BuyPrice = 2, InvestedMoney = 5, TypeId = 2},
            };
        }

        public static TheoryData<AddAssetFormModel> IncorrectInputData()
        {
            return new TheoryData<AddAssetFormModel>
            {
                new AddAssetFormModel {Name = "1", BuyPrice = 1, InvestedMoney = 0, TypeId = 3},
                new AddAssetFormModel {Name = "2", BuyPrice = 2, InvestedMoney = -1, TypeId = 4},

            };
        }

        public static TheoryData<SellAssetServiceModel> SellAssetData()
        {
            return new TheoryData<SellAssetServiceModel>
            {
                new SellAssetServiceModel {Name = "1", BuyPrice = 1, InvestedMoney = 5},
                new SellAssetServiceModel {Name = "2", BuyPrice = 2, InvestedMoney = 5}
            };
        }

        //Tests for AddAsset() ---------------------------------------------------------------------------------
        //GET Method
        [Fact]
        public void AddAssetShouldReturnView()
        {
            //Arrange
            var userId = "valid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = assetsController.AddAsset();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;
            Assert.IsType<AddAssetFormModel>(model);
        }

        //POST Method
        [Theory]
        [MemberData(nameof(CorrectInputData))]
        public void AddAssetShouldRedirectToAllOnCorrectInput(AddAssetFormModel input)
        {
            //Arrange
            var userId = "valid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = (RedirectToActionResult)assetsController.AddAsset(input);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("All", result.ActionName);
        }

        [Theory]
        [MemberData(nameof(IncorrectInputData))]
        public void AddAssetShouldReturnViewOnIncorrectType(AddAssetFormModel input)
        {
            //Arrange
            var userId = "valid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = assetsController.AddAsset(input);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;
            Assert.IsType<AddAssetFormModel>(model);
        }

        [Theory]
        [MemberData(nameof(CorrectInputData))]
        public void AddAssetShouldReturnViewWhenNotEnoughAvailableMoney(AddAssetFormModel input)
        {
            //Arrange
            var userId = "nonValid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = assetsController.AddAsset(input);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;
            Assert.IsType<AddAssetFormModel>(model);
        }

        //Tests for All() ---------------------------------------------------------------------------------
        //GET Method
        [Theory]
        [InlineData(null, 3)]
        [InlineData("Name", 5)]
        public void AllShouldReturnAllViewOnCorrectInput(string sortOrder, int pageSize)
        {
            //Arrange
            var userId = "valid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = assetsController.All(null, 1);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;
            Assert.IsType<AllAssetsListViewModel>(model);
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData("Name", 5)]
        public void AllShouldReturnNoAssetsViewWhenUserHasNoAssets(string sortOrder, int pageSize)
        {
            //Arrange
            var userId = "nonValid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = assetsController.All(null, 1);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("NoAssets", viewResult.ViewName);
        }

        //POST Method
        [Theory]
        [InlineData(5)]
        [InlineData(3)]
        public void AllShouldRedirectToAllWithMethodGet(int id)
        {
            //Arrange
            var userId = "nonValid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = (RedirectToActionResult)assetsController.All(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("All", result.ActionName);
        }

        //Test for Sell() -------------------------------------------------------------------------------------
        //GET Method
        [Theory]
        [InlineData(1)]
        public void SellShouldReturnViewWhenAssetBelongsToUser(int id)
        {
            //Arrange
            var userId = "valid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = assetsController.Sell(id);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;
            Assert.IsType<SellAssetServiceModel>(model);
        }

        [Theory]
        [InlineData(2)]
        public void SellShouldReturnUnauthorizedWhenAssetDoesntBelongToUser(int id)
        {
            //Arrange
            var userId = "valid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = assetsController.Sell(id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }

        //POST Method
        [Theory]
        [MemberData(nameof(SellAssetData))]
        public void SellShouldRedirectToAllAction(SellAssetServiceModel asset)
        {
            //Arrange
            var userId = "valid";
            var assetsController = ArrangeTest(userId);

            //Act
            var result = (RedirectToActionResult)assetsController.Sell(asset);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("All", result.ActionName);
        }
    }
}
