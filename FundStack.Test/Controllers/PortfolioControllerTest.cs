using FundStack.Controllers;
using FundStack.Models.Portfolio;
using FundStack.Services.Portfolios;
using FundStack.Test.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace FundStack.Test.Controllers
{
    public class PortfolioControllerTest
    {
        //Helper Classes
        private PortfolioController ArrangeTest(string userId)
        {
            var portfolioService = PortfolioServiceMock.Instance;
            var portfolioController = new PortfolioController(portfolioService);

            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = MockUser(userId) }
            };

            return portfolioController;
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

        public static TheoryData<AddMoneyViewModel> CorrectMoneyData()
        {
            return new TheoryData<AddMoneyViewModel>
            {
                new AddMoneyViewModel {UserId = "1", Money = 1},
                new AddMoneyViewModel {UserId = "100", Money = 100},
            };
        }

        public static TheoryData<AddMoneyViewModel> IncorrectMoneyData()
        {
            return new TheoryData<AddMoneyViewModel>
            {
                new AddMoneyViewModel {UserId = "0", Money = 0},
                new AddMoneyViewModel {UserId = "-10", Money = -10},
            };
        }

        //Tests for Value()  ---------------------------------------------------------------------------------------
        [Fact]
        public void ValueShouldReturnViewWhenAvailableMoneyIsMoreThannonValid()
        {
            //Arrange
            var userId = "valid";
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = portfolioController.Value();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            
            var model = viewResult.Model;
            Assert.IsType<ValuePortfolioServiceModel>(model);
        }

        [Fact]
        public void ValueShouldReturnErrorViewWhenAvailableMoneyIsnonValid()
        {
            //Arrange
            var userId = "nonValid";
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = portfolioController.Value();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("NoFunds", viewResult.ViewName);
        }

        //Tests for AddMoney() ---------------------------------------------------------------------------------------
        [Theory]
        [MemberData(nameof(CorrectMoneyData))]
        public void AddMoneyShouldRedirectToValueOnCorrectInput(AddMoneyViewModel addedMoney)
        {
            //Arrange
            var userId = addedMoney.UserId;
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = (RedirectToActionResult)portfolioController.AddMoney(addedMoney);

            //Assert 
            Assert.NotNull(result);
            Assert.Equal("Value", result.ActionName);
        }

        [Theory]
        [MemberData(nameof(IncorrectMoneyData))]
        public void AddMoneyShouldReturnViewOnIncorrectInput(AddMoneyViewModel addedMoney)
        {
            //Arrange
            var userId = addedMoney.UserId;
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = portfolioController.AddMoney(addedMoney);

            //Assert 
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        //Tests for WithdrawMoney()  ---------------------------------------------------------------------------------------
        [Theory]
        [MemberData(nameof(CorrectMoneyData))]
        public void WithdrawMoneyShouldRedirectToValueOnCorrectInput(AddMoneyViewModel withdrawMoney)
        {
            //Arrange
            var userId = "valid";
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = (RedirectToActionResult)portfolioController.WithdrawMoney(withdrawMoney);

            //Assert 
            Assert.NotNull(result);
            Assert.Equal("Value", result.ActionName);
        }

        [Theory]
        [MemberData(nameof(IncorrectMoneyData))]
        public void WithdrawMoneyShouldReturnViewOnIncorrectInput(AddMoneyViewModel withdrawMoney)
        {
            //Arrange
            var userId = "valid";
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = portfolioController.WithdrawMoney(withdrawMoney);

            //Assert 
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [MemberData(nameof(CorrectMoneyData))]
        public void WithdrawMoneyShouldReturnViewWhenNotEnoughAvailableMoney(AddMoneyViewModel withdrawMoney)
        {
            //Arrange
            var userId = "nonValid";
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = portfolioController.WithdrawMoney(withdrawMoney);

            //Assert 
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        //Tests for Statistics()  ---------------------------------------------------------------------------------------
        [Fact]
        public void StatisticsShouldReturnNoAssetsView()
        {
            //Arrange
            var userId = "nonValid";
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = portfolioController.Statistics();

            //Assert 
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("NoAssets", viewResult.ViewName);
        }

        [Fact]
        public void StatisticsShouldReturnViewWithStats()
        {
            //Arrange
            var userId = "valid";
            var portfolioController = ArrangeTest(userId);

            //Act
            var result = portfolioController.Statistics();

            //Assert 
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
 
            var model = viewResult.Model;
            Assert.IsType<PortfolioStatisticServiceModel>(model);
        }
    }
}
