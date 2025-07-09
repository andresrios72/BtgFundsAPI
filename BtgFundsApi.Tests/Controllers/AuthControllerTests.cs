using BtgFundsApi.Configurations;
using BtgFundsApi.Controllers;
using BtgFundsApi.DTOs;
using BtgFundsApi.Models;
using BtgFundsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace BtgFundsApi.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]        
        public async Task Login_ReturnsJwtToken_WhenCredentialsAreValid()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "ClaveSuperSecretaDePruebaParaJWTGFT2025!!",
                Issuer = "BtgFundsApi",
                Audience = "BtgFundsApiClient"
            });

            var plainPassword = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
                PasswordHash = hashedPassword,
                Role = "user"
            };

            mockUserService.Setup(s => s.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            var controller = new AuthController(mockUserService.Object, jwtSettings);

            var request = new LoginRequest
            {
                Email = user.Email,
                Password = plainPassword
            };

            // Act
            var result = await controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            string token = doc.RootElement.GetProperty("token").GetString();

            Assert.False(string.IsNullOrEmpty(token));
        }


    }
}