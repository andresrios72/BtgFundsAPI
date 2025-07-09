using BtgFundsApi.Controllers;
using BtgFundsApi.DTOs;
using BtgFundsApi.Models;
using BtgFundsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BtgFundsApi.Tests.Controllers
{
    public class FundsControllerTests
    {
        [Fact]
        public async Task Get_ReturnsListOfFunds()
        {
            // Arrange
            var mockFundService = new Mock<IFundService>();
            var mockUserService = new Mock<IUserService>();
            var mockTransactionService = new Mock<ITransactionService>();

            var funds = new List<Fund>
            {
                new Fund { Id = 1, Name = "Fund 1", MinAmount = 1000, Category = "FPV" },
                new Fund { Id = 2, Name = "Fund 2", MinAmount = 2000, Category = "FPV" }
            };

            mockFundService.Setup(s => s.GetAsync()).ReturnsAsync(funds);

            var controller = new FundsController(mockFundService.Object, mockUserService.Object, mockTransactionService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<Fund>>>(result);
            var returnValue = Assert.IsType<List<Fund>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal("Fund 1", returnValue[0].Name);
        }

        [Fact]
        public async Task Subscribe_ReturnsBadRequest_WhenInsufficientBalance()
        {
            // Arrange
            var mockFundService = new Mock<IFundService>();
            var mockUserService = new Mock<IUserService>();
            var mockTransactionService = new Mock<ITransactionService>();

            var user = new User
            {
                Id = "user123",
                Name = "Andrés",
                Balance = 1000,
                FundsSubscribed = new List<UserFundSubscription>()
            };

            var fundList = new List<Fund>
            {
                new Fund { Id = 1, Name = "Fund 1", MinAmount = 500, Category = "FPV" }
            };

            mockUserService.Setup(s => s.GetByIdAsync(user.Id)).ReturnsAsync(user);
            mockFundService.Setup(s => s.GetAsync()).ReturnsAsync(fundList);

            var controller = new FundsController(
                mockFundService.Object,
                mockUserService.Object,
                mockTransactionService.Object);

            var request = new SubscribeFundRequest
            {
                UserId = user.Id,
                FundId = 1,
                Amount = 2000 // mayor al saldo
            };

            // Act
            var result = await controller.Subscribe(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("No tiene saldo suficiente", badRequestResult.Value.ToString());
        }
    }
}
