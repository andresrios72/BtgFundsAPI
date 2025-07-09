using Xunit;
using Moq;
using MongoDB.Driver;
using BtgFundsApi.Services;
using BtgFundsApi.Models;
using Microsoft.Extensions.Options;
using BtgFundsApi.Configurations;
using System.Threading.Tasks;
using System.Threading;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BtgFundsApi.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectUser()
        {
            // Arrange
            var mockCollection = new Mock<IMongoCollection<User>>();
            var mockCursor = new Mock<IAsyncCursor<User>>();
            var user = new User { Id = "user123", Name = "Andrés", Email = "andres@example.com" };

            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
            mockCursor.Setup(c => c.Current).Returns(new List<User> { user });

            mockCollection
                .Setup(op => op.FindAsync(
                    It.IsAny<FilterDefinition<User>>(),
                    It.IsAny<FindOptions<User, User>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            var mockClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(d => d.GetCollection<User>("users", null)).Returns(mockCollection.Object);
            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);

            var settings = Options.Create(new MongoDBSettings { DatabaseName = "btg_funds_db" });
            var userService = new UserService(settings, mockClient.Object);

            // Act
            var result = await userService.GetByIdAsync("user123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Andrés", result.Name);
            Assert.Equal("andres@example.com", result.Email);
        }
    }
}
