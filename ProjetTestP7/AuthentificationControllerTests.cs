using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NouveauP7API.Domain;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using Xunit;

namespace NouveauP7API.Tests.Controllers
{
    public class AuthentificationControllerTests
    {
        // Test method for the Login action
        [Fact]
        public async Task Login_ValidModel_ReturnsOkResultWithToken()
        {
            // Arrange
            var model = new LoginModel { /* Initialize with valid properties */ };
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByCredentialsAsync(model.Username)).ReturnsAsync((User)null);
            userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<(string, string, string)>())).Returns(Task.CompletedTask);
            var jwtFactoryMock = new Mock<IJwtFactory>();
            jwtFactoryMock.Setup(factory => factory.GeneratedEncodedToken(It.IsAny<(string Username, string Email, string Password)>())).Returns("testToken");
            var controller = new AuthentificationController(userRepositoryMock.Object, jwtFactoryMock.Object);

            // Act
            var result = await controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Utilisateur créé et connecté avec succès", okResult.Value.GetType().GetProperty("Message").GetValue(okResult.Value));
            Assert.Equal("testToken", okResult.Value.GetType().GetProperty("Token").GetValue(okResult.Value));
        }

        // Add more test methods to cover additional scenarios for the Login action
        // Test method for the Login action when ModelState is not valid
        [Fact]
        public async Task Login_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new LoginModel { /* Initialize with invalid properties */ };
            var userRepositoryMock = new Mock<IUserRepository>();
            var jwtFactoryMock = new Mock<IJwtFactory>();
            var controller = new AuthentificationController(userRepositoryMock.Object, jwtFactoryMock.Object);
            controller.ModelState.AddModelError("TestError", "ModelState error message");

            // Act
            var result = await controller.Login(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        // Add more test methods to cover additional scenarios for the Login action
    }

}


