using Xunit;
using Moq;
using MongoDB.Driver;
using BtgFundsApi.Services;
using BtgFundsApi.Models;
using Microsoft.Extensions.Options;
using BtgFundsApi.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BtgFundsApi.Tests.Services
{
    public class FundServiceTests
    {
        [Fact]
        public async Task GetAsync_ReturnsListOfFunds()
        {
            // Arrange
            var funds = new List<Fund>
            {
                new Fund { Id = 1, Name = "Test Fondo 1", MinAmount = 1000, Category = "FPV" },
                new Fund { Id = 2, Name = "Test Fondo 2", MinAmount = 2000, Category = "FPV" }
            };

            var mockCollection = new Mock<IMongoCollection<Fund>>();
            var mockCursor = new Mock<IAsyncCursor<Fund>>();
            mockCursor.Setup(_ => _.Current).Returns(funds);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            mockCollection
                .Setup(op => op.FindAsync(
                    It.IsAny<FilterDefinition<Fund>>(),
                    It.IsAny<FindOptions<Fund, Fund>>(),
                    default))
                .ReturnsAsync(mockCursor.Object);

            var mockClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();

            mockDatabase.Setup(d => d.GetCollection<Fund>("funds", null)).Returns(mockCollection.Object);
            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);

            var settings = Options.Create(new MongoDBSettings { DatabaseName = "btg_funds_db" });

            var service = new FundService(settings, mockClient.Object);

            // Act
            var result = await service.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Fondo 1", result[0].Name);
        }
    }
}
