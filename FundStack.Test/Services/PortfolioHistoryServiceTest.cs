using FundStack.Data.Models;
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
                    
                }
            });

            //Act


            //Assert
        }
    }
}
