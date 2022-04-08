using FundStack.Controllers;
using FundStack.Services.AssetsHistory;
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
    public class AssetsHistoryControllerTest
    {
        //Helper Classes
        private AssetsHistoryController ArrangeTest(string userId)
        {
            var assetHistoryService = AssetHistoryServiceMock.Instance;
            var assetsHistoryController = new AssetsHistoryController(assetHistoryService);

            assetsHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = MockUser(userId) }
            };

            return assetsHistoryController;
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

        //Tests of AllHistory()
        [Fact]
        public void AllHistoryShouldReturnView()
        {
            //Arrange
            var userId = "valid";
            var assetsHistoryController = ArrangeTest(userId);

            //Act
            var result = assetsHistoryController.AllHistory();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;
            Assert.IsType<List<AssetHistoryServiceModel>>(model);
        }

        //Tests of Export()
        [Fact]
        public void ExportShouldReturnFile()
        {
            //Arrange
            var userId = "valid";
            var assetsHistoryController = ArrangeTest(userId);

            //Act
            var result = assetsHistoryController.Export();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<FileContentResult>(result);
        }

    }
}
