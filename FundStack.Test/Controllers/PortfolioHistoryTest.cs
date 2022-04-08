using FundStack.Controllers;
using FundStack.Models.PortfolioHistory;
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
    public class PortfolioHistoryTest
    {
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

        [Fact]
        public void TotalValue()
        {
            //Arrange
            var portfolioHistoryMock = PortfolioHistoryServiceMock.Instance;
            var assetsController = new PortfolioHistoryController(portfolioHistoryMock);

            assetsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = MockUser("valid") }
            };

            //Act
            var result = assetsController.TotalValue();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;
            Assert.IsType<PortfolioHistoryViewModel>(model);
        }
    }
}
