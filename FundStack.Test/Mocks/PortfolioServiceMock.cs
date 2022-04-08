using System.Collections.Generic;
using FundStack.Data.Models;
using FundStack.Services.Portfolios;
using Moq;

namespace FundStack.Test.Mocks
{
    public static class PortfolioServiceMock
    {
        public static IPortfolioService Instance
        {
            get
            {
                var portfolioServiceMock = new Mock<IPortfolioService>();

                //GetCurrentPortfolio
                portfolioServiceMock
                    .Setup(p => p.GetCurrentPortfolio("valid"))
                    .Returns(new Portfolio
                    {
                        UserId = "valid",
                        AvailableMoney = 200,
                        Assets = new[]
                        {
                            new Asset
                            {
                                Name = "testAsset"
                            }
                        }
                    });

                portfolioServiceMock
                    .Setup(p => p.GetCurrentPortfolio("nonValid"))
                    .Returns(new Portfolio
                    {
                        UserId = "nonValid",
                        AvailableMoney = 0
                    });

                //Details
                portfolioServiceMock
                    .Setup(p => p.Details("valid"))
                    .Returns(new ValuePortfolioServiceModel
                    {
                        UserId = "valid",
                        AvailableMoney = 200,
                    });

                portfolioServiceMock
                   .Setup(p => p.Details("nonValid"))
                   .Returns(new ValuePortfolioServiceModel
                   {
                       UserId = "nonValid",
                       AvailableMoney = 0
                   });

                //GetPortfolioStats
                portfolioServiceMock
                 .Setup(p => p.GetPortfolioStats("valid"))
                 .Returns(new PortfolioStatisticServiceModel
                 {
                     CryptoStatistics = new Dictionary<string, decimal>
                     {
                         { "testStats", 150 }
                 }
                 });

                return portfolioServiceMock.Object;
            }
        }
    }
}
