using FundStack.Data.Models;
using FundStack.Services.PortfoliosHistory;
using FundStack.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FundStack.Test.Services
{
    public class PortfolioHistoryServiceTest
    {
        [Fact]
        public void TotalValueShouldReturnPortfolioHostoryOfCorrectUser()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var userId = "testId";


            data.Users.Add(new User()
            {
                Id = userId,
                Portfolio = new Portfolio()
                {
                    UserId = userId,
                    PortfolioHistory = new List<PortfolioHistory>()
                    {
                       new PortfolioHistory { 
                           SnapshotDate = Convert.ToDateTime("03/22/2022"),
                           PortfolioValue = 1500
                       },
                       new PortfolioHistory {
                           SnapshotDate = Convert.ToDateTime("03/23/2022"),
                           PortfolioValue = 1600
                       },
                       new PortfolioHistory {
                           SnapshotDate = Convert.ToDateTime("03/24/2022"),
                           PortfolioValue = 1550
                       },
                    }
                }
            });

            data.SaveChanges();
            var portfolioHistoryService = new PortfolioHistoryService(data);

            //Act
            var result = portfolioHistoryService.TotalValue(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<PortfolioHistoryServiceModel>>(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(1500, result.First().PortfolioValue);
        }

        [Fact]
        public void RecordValuesSavesCurrentTotalValueOfPortfolio()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var userId = "testId";

            data.Users.Add(new User()
            {
                Id = userId,
                Portfolio = new Portfolio()
                {
                    UserId = userId,
                    PortfolioHistory = new List<PortfolioHistory>()
                    {
                       new PortfolioHistory {
                           SnapshotDate = Convert.ToDateTime("03/22/2022"),
                           PortfolioValue = 1500
                       },
                    },
                    TotalValue = 1550
                }
            });

            data.SaveChanges();
            var user = data.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();
            var history = user.Portfolio.PortfolioHistory;

            var portfolioHistoryService = new PortfolioHistoryService(data);

            //Act
            portfolioHistoryService.RecordPorfoltioValue();

            //Assert
            Assert.NotNull(history);
            Assert.Equal(2, history.Count());
            Assert.Equal(1500, history.First().PortfolioValue);
            Assert.Equal(1550, history.Last().PortfolioValue);
        }
    }
}
