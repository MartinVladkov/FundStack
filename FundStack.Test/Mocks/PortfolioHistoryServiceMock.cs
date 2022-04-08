using FundStack.Services.PortfoliosHistory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundStack.Test.Mocks
{
    public static class PortfolioHistoryServiceMock
    {
        public static IPortfolioHistoryService Instance
        {
            get
            {
                var portfolioHistoryServiceMock = new Mock<IPortfolioHistoryService>();

                //TotalValue()
                portfolioHistoryServiceMock
                    .Setup(p => p.TotalValue("valid"))
                    .Returns(new List<PortfolioHistoryServiceModel>
                    {
                       new PortfolioHistoryServiceModel { PortfolioId = "1",
                        PortfolioValue = 250,
                        SnapshotDate = DateTime.UtcNow }
                    });

                return portfolioHistoryServiceMock.Object;
            }
        }
    }
}
