using FundStack.Data.Models;
using FundStack.Models.Assets;
using FundStack.Services.Assets;
using FundStack.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FundStack.Test.Services
{
    public class AssetServiceTest
    {
        public static TheoryData<AddAssetFormModel> AddAssetData =>
        new TheoryData<AddAssetFormModel>
        {
            new AddAssetFormModel { Name = "test1", BuyPrice = 25, InvestedMoney = 50, TypeId = 1},
            new AddAssetFormModel { Name = "test2", BuyPrice = 10, InvestedMoney = 30, TypeId = 2},
        };

        [Theory]
        [MemberData(nameof(AddAssetData))]
        public void AddAssetShouldBeAddedCorrectly(AddAssetFormModel asset)
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var userId = "testId";

            data.Users.Add(new User()
            {
                Id = userId,
                Portfolio = new Portfolio()
                {
                    AvailableMoney = 100
                }
            });

            data.SaveChanges();

            var user = data.Users.Where(u => u.Id == userId).FirstOrDefault();
            var expectedAvailableMoney = 100 - asset.InvestedMoney;

            var assetService = new AssetService(data);

            //Act
            assetService.AddAsset(userId, asset);

            //Assert
            Assert.Equal(userId, user.Id);
            Assert.Equal(1, user.Portfolio.Assets.Count());
            Assert.Equal(expectedAvailableMoney, user.Portfolio.AvailableMoney);
            Assert.True(user.Portfolio.Assets.Any(a => a.Name == asset.Name.ToUpper()));
        }

        [Theory]
        [InlineData("id1", 0, 3, "Name")]
        [InlineData("id2", 1, 3, "profitLoss_desc")]
        public void AllShouldReturnSortedPageWithAssets(string userId, int excludeRecords, int pageSize, string sortOrder)
        {
            //Arrange
            using var data = DatabaseMock.Instance;

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
                        new Asset{ Name = "aa", TypeId = 1, ProfitLoss = 10}, 
                        new Asset{ Name = "bb", TypeId = 2, ProfitLoss = 15},
                        new Asset{ Name = "cc", TypeId = 1, ProfitLoss = -20},
                        new Asset{ Name = "dd", TypeId = 3, ProfitLoss = 18}, 
                    }
                }
            });

            data.SaveChanges();

            var assetService = new AssetService(data);

            //Act
            var result = assetService.All(userId, excludeRecords, pageSize, sortOrder);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<AllAssetServiceModel>>(result);
            Assert.Equal(3, result.Count());
            if(sortOrder == "Name")
            {
                Assert.Equal("AA", result.First().Name);
            }
            if(sortOrder == "profitLoss_desc")
            {
                Assert.Equal(15, result.First().ProfitLoss);
                Assert.Equal(-20, result.Last().ProfitLoss);
            }
        }

        [Fact]
        public void UpdateDatabaseShouldFetchValuesFromApiAndUpdateDatabase()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var userId = "testId";

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
                        new Asset{ Name = "BTC", TypeId = 1, BuyPrice = 40000, InvestedMoney = 40000},
                        new Asset{ Name = "TSLA", TypeId = 2, BuyPrice = 1050, InvestedMoney = 1050},
                        new Asset{ Name = "VTI", TypeId = 3, BuyPrice = 225, InvestedMoney = 225},
                    }
                }
            });

            data.SaveChanges();

            var user = data.Users.Where(u => u.Id == userId).FirstOrDefault();
            var btc = user.Portfolio.Assets.Where(a => a.Name == "BTC").FirstOrDefault();
            var tsla = user.Portfolio.Assets.Where(a => a.Name == "TSLA").FirstOrDefault(); ;
            var vti = user.Portfolio.Assets.Where(a => a.Name == "VTI").FirstOrDefault(); ;

            var assetService = new AssetService(data);

            //Act
            assetService.UpdateDatabase(userId);

            //Assert
            Assert.NotNull(btc.CurrentPrice);
            Assert.NotNull(btc.ProfitLoss);
            Assert.NotNull(btc.ProfitLossPercent);
            Assert.NotNull(btc.Amount);

            Assert.NotNull(tsla.CurrentPrice);
            Assert.NotNull(tsla.ProfitLoss);
            Assert.NotNull(tsla.ProfitLossPercent);
            Assert.NotNull(tsla.Amount);

            Assert.NotNull(vti.CurrentPrice);
            Assert.NotNull(vti.ProfitLoss);
            Assert.NotNull(vti.ProfitLossPercent);
            Assert.NotNull(vti.Amount);
        }


        [Fact]
        public void SellShouldMoveAssetFromAssetsToAssetsHistoryTable()
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
                    AvailableMoney = 10000,
                    Assets = new List<Asset>
                    {
                        new Asset{ Id = 1, Name = "BTC", TypeId = 1, BuyPrice = 40000, 
                            InvestedMoney = 40000, CurrentPrice = 40100, ProfitLoss = 100, ProfitLossPercent = 2, Amount = 1},
                        new Asset{ Id = 2, Name = "TSLA", TypeId = 2, BuyPrice = 1050, InvestedMoney = 1050},
                        new Asset{ Id = 3, Name = "VTI", TypeId = 3, BuyPrice = 225, InvestedMoney = 225},
                    }
                }
            }) ;

            data.SaveChanges();

            var assetToSell = new SellAssetServiceModel
            {
                Id = 1,
                PortfolioId = userId,
                Name = "BTC",
                BuyPrice = 40000,
                InvestedMoney = 40000
            };

            var user = data.Users.Where(u => u.Id == userId).FirstOrDefault();

            var assetService = new AssetService(data);

            //Act
            assetService.Sell(assetToSell, userId);

            //Assert
            var soldBtc = user.Portfolio.AssetsHistory.Where(a => a.Name == "BTC").FirstOrDefault();
            Assert.Equal(2, user.Portfolio.Assets.Count());
            Assert.Equal(1, user.Portfolio.AssetsHistory.Count());
            Assert.Equal(40100, soldBtc.SellPrice);
            Assert.Equal(DateTime.UtcNow.Date, soldBtc.SellDate.Date);
            Assert.Equal(50100, user.Portfolio.AvailableMoney);
        }

        [Fact]
        public void DeleteShouldRemoveAssetFromTable()
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
                    AvailableMoney = 10000,
                    Assets = new List<Asset>
                    {
                        new Asset{ Id = 1, Name = "BTC", TypeId = 1, BuyPrice = 40000, InvestedMoney = 40000},
                        new Asset{ Id = 2, Name = "TSLA", TypeId = 2, BuyPrice = 1050, InvestedMoney = 1050},
                        new Asset{ Id = 3, Name = "VTI", TypeId = 3, BuyPrice = 225, InvestedMoney = 225},
                    }
                }
            });

            data.SaveChanges();

            var user = data.Users.Where(u => u.Id == userId).FirstOrDefault();

            var assetService = new AssetService(data);

            //Act
            assetService.Delete(1, userId);

            //Assert
            Assert.Equal(2, user.Portfolio.Assets.Count());
            Assert.Equal(50000, user.Portfolio.AvailableMoney);
        }

        [Fact]
        public void GetAssetTypesReturnsTypesFromDatabase()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var userId = "testId";

            data.Types.Add(new Data.Models.Type { Id = 1, Name = "Crypto" });
            data.Types.Add(new Data.Models.Type { Id = 2, Name = "Stock" });
            data.Types.Add(new Data.Models.Type { Id = 3, Name = "ETF" });

            data.SaveChanges();

            var assetService = new AssetService(data);

            //Act
            var result = assetService.GetAssetTypes();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<AssetTypeViewModel>>(result);
            Assert.Equal(3, result.Count());
            Assert.True(result.Any(t => t.Name == "Crypto"));
            Assert.True(result.Any(t => t.Name == "Stock"));
            Assert.True(result.Any(t => t.Name == "ETF"));
        }

        [Fact]
        public void GetCountShouldReturnCorrectCount()
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
                    AvailableMoney = 10000,
                    Assets = new List<Asset>
                    {
                        new Asset{ Id = 1, Name = "BTC", TypeId = 1, BuyPrice = 40000, InvestedMoney = 40000},
                        new Asset{ Id = 2, Name = "TSLA", TypeId = 2, BuyPrice = 1050, InvestedMoney = 1050},
                        new Asset{ Id = 3, Name = "VTI", TypeId = 3, BuyPrice = 225, InvestedMoney = 225},
                    }
                }
            });

            data.SaveChanges();

            var assetService = new AssetService(data);

            //Act
            var result = assetService.GetCount(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<int>(result);
            Assert.Equal(3, result);
        }

        [Fact]
        public void CheckNullAssetPriceReturnsFalseIfAllAssetsHaveCurrentPrice()
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
                    Assets = new List<Asset>
                    {
                        new Asset{ Id = 1, Name = "BTC", CurrentPrice = 250},
                        new Asset{ Id = 2, Name = "TSLA", CurrentPrice = 250},
                        new Asset{ Id = 3, Name = "VTI", CurrentPrice = 250},
                    }
                }
            });

            data.SaveChanges();

            var assetService = new AssetService(data);

            //Act
            var result = assetService.CheckNullAssetPrice(userId);

            //Assert
            Assert.NotNull(result);
            Assert.False(result);
        }


        [Fact]
        public void CheckNullAssetPriceReturnsTrueIfAssetHasNoCurrentPrice()
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
                    Assets = new List<Asset>
                    {
                        new Asset{ Id = 1, Name = "BTC", CurrentPrice = 250},
                        new Asset{ Id = 2, Name = "TSLA", CurrentPrice = 250},
                        new Asset{ Id = 3, Name = "VTI"},
                    }
                }
            });

            data.SaveChanges();

            var assetService = new AssetService(data);

            //Act
            var result = assetService.CheckNullAssetPrice(userId);

            //Assert
            Assert.NotNull(result);
            Assert.True(result);
        }
    }
}
