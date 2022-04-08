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
                    .Setup(p => p.GetCurrentPortfolio("nonZero"))
                    .Returns(new Portfolio
                    {
                        UserId = "nonZero",
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
                    .Setup(p => p.GetCurrentPortfolio("zero"))
                    .Returns(new Portfolio
                    {
                        UserId = "zero",
                        AvailableMoney = 0
                    });

                //Details
                portfolioServiceMock
                    .Setup(p => p.Details("nonZero"))
                    .Returns(new ValuePortfolioServiceModel
                    {
                        UserId = "nonZero",
                        AvailableMoney = 200,
                    });

                portfolioServiceMock
                   .Setup(p => p.Details("zero"))
                   .Returns(new ValuePortfolioServiceModel
                   {
                       UserId = "zero",
                       AvailableMoney = 0
                   });

                //GetPortfolioStats
                portfolioServiceMock
                 .Setup(p => p.GetPortfolioStats("nonZero"))
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
