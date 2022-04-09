using FundStack.Data.Models;
using FundStack.Services.Portfolios;
using FundStack.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FundStack.Test.Services
{
    public class PortfolioServiceTest
    {
        public static TheoryData<List<decimal>> Data =>
        new TheoryData<List<decimal>>
        {
            new List<decimal> { 1, 10, 20, 30, 40, 50, 60 },
            new List<decimal> { 2, 100, 150, 200, 250, 300, 350 },
        };

        [Theory]
        [InlineData("id1", 130)]
        [InlineData("id2", 50.5)]
        public void AddMoneyIncreasesAvailableMoneyForCorrectUser(string userId, decimal addedMoney)
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            data.Users.Add(new User()
            {
                Id = userId,
                Portfolio = new Portfolio()
                {
                    AvailableMoney = 50
                }
            });
            data.SaveChanges();
            var user = data.Users.Where(u => u.Id == userId).FirstOrDefault();
            var expectedResult = addedMoney + user.Portfolio.AvailableMoney;

            var portfolioService = new PortfolioService(data);

            //Act
            portfolioService.AddMoney(userId, addedMoney);

            //Assert
            Assert.Equal(expectedResult, user.Portfolio.AvailableMoney);
        }

        [Theory]
        [InlineData("id1", 130)]
        [InlineData("id2", 50.5)]
        public void WithdrawMoneyDecreasesAvailableMoneyForCorrectUser(string userId, decimal wihdrawMoney)
        {
            //Arrange
            using var data = DatabaseMock.Instance;

            data.Users.Add(new User()
            {
                Id = userId,
                Portfolio = new Portfolio()
                {
                    AvailableMoney = 200
                }
            });

            data.SaveChanges();
            var user = data.Users.Where(u => u.Id == userId).FirstOrDefault();
            var expectedResult = user.Portfolio.AvailableMoney - wihdrawMoney;

            var portfolioService = new PortfolioService(data);

            //Act
            portfolioService.WithdrawMoney(userId, wihdrawMoney);

            //Assert
            Assert.Equal(expectedResult, user.Portfolio.AvailableMoney);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DetailsReturnsCorrectPortfolio(List<decimal> assets)
        {
            //Arrange
            using var data = DatabaseMock.Instance;

            var available = 200;
            var userId = assets[0].ToString();

            data.Users.Add(new User()
            {
                Id = userId, 
                Portfolio = new Portfolio()
                {
                    AvailableMoney = available,
                    Assets = new List<Asset>
                    {
                        new Asset {Name = "asset1", InvestedMoney = assets[3], ProfitLoss = -20},
                        new Asset {Name = "asset2", InvestedMoney = assets[4], ProfitLoss = 15},
                        new Asset {Name = "asset3", InvestedMoney = assets[5], ProfitLoss = 10}
                    }
                }
            });

            data.SaveChanges();

            var expectedInvested = assets[3] + assets[4] + assets[5];
            var expectedProfitLoss = 5;
            var expectedProfitLossPercent = (expectedProfitLoss / (expectedInvested + available)) * 100;
            var expectedTotal = available + expectedInvested + expectedProfitLoss;
            var portfolioService = new PortfolioService(data);

            //Act
            var result = portfolioService.Details(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ValuePortfolioServiceModel>(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(available, result.AvailableMoney);
            Assert.Equal(expectedInvested, result.InvestedMoney);
            Assert.Equal(expectedProfitLoss, result.ProfitLoss);
            Assert.Equal(expectedProfitLossPercent, result.ProfitLossPercent);
            Assert.Equal(expectedTotal, result.TotalValue);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetPortfolioStats(List<decimal> assets)
        {
            //Arrange
            using var data = DatabaseMock.Instance;

            var userId = assets[0].ToString();
            data.Types.Add(new Data.Models.Type { Id = 1, Name = "Crypto" });
            data.Types.Add(new Data.Models.Type { Id = 2, Name = "Stock" });
            data.Types.Add(new Data.Models.Type { Id = 3, Name = "ETF" });
            data.Users.Add(new User()
            {
                Id = userId,
                Portfolio = new Portfolio()
                {
                    UserId = userId,
                    Assets = new List<Asset>
                    {
                        new Asset {Name = "asset1", TypeId = 1, InvestedMoney = assets[1]},
                        new Asset {Name = "asset2", TypeId = 1, InvestedMoney = assets[2]},
                        new Asset {Name = "asset3", TypeId = 2, InvestedMoney = assets[3]},
                        new Asset {Name = "asset4", TypeId = 2, InvestedMoney = assets[4]},
                        new Asset {Name = "asset5", TypeId = 3, InvestedMoney = assets[5]},
                        new Asset {Name = "asset6", TypeId = 3, InvestedMoney = assets[6]},
                    }
                }
            }); ;

            data.SaveChanges();

            var group1 = assets[1] + assets[2];
            var group2 = assets[3] + assets[4];
            var group3 = assets[5] + assets[6];
            var total = assets.Sum();
            var portfolioService = new PortfolioService(data);

            //Act
            var result = portfolioService.GetPortfolioStats(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<PortfolioStatisticServiceModel>(result);
            Assert.Equal(2, result.CryptoStatistics.Values.Count());
            Assert.Equal(2, result.StockStatistics.Values.Count());
            Assert.Equal(2, result.EtfStatistics.Values.Count());
            Assert.Contains(assets[1], result.CryptoStatistics.Values);
            Assert.Contains(assets[2], result.CryptoStatistics.Values);
            Assert.Contains(assets[3], result.StockStatistics.Values);
            Assert.Contains(assets[4], result.StockStatistics.Values);
            Assert.Contains(assets[5], result.EtfStatistics.Values);
            Assert.Contains(assets[6], result.EtfStatistics.Values);
            Assert.Contains(group1, result.TotalStatistics.Values);
            Assert.Contains(group2, result.TotalStatistics.Values);
            Assert.Contains(group3, result.TotalStatistics.Values);
        }
    }
}
