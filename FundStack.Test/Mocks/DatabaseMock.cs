using FundStack.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace FundStack.Test.Mocks
{
    public static class DatabaseMock
    {
        public static FundStackDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<FundStackDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new FundStackDbContext(dbContextOptions);
            }
        }
    }
}
