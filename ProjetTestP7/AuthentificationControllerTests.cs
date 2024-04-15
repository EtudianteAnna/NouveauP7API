using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NouveauP7API.Controllers;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using Xunit;

namespace NouveauP7API.Tests.Controllers
{
    public class AuthentificationControllerTests
    {
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null, null);
            var jwtFactoryMock = new Mock<IJwtFactory>();
            var controller = new AuthentificationController(userManagerMock.Object, jwtFactoryMock.Object);

            var loginModel = new LoginModel
            {
                Username = "validUsername",
                Password = "validPassword"
            };

            var user = new User
            {
                UserName = loginModel.Username,
                Email = "valid@email.com"
            };

            userManagerMock.Setup(um => um.FindByNameAsync(loginModel.Username))
                .ReturnsAsync(user);
            userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginModel.Password))
                .ReturnsAsync(true);
            userManagerMock.Setup(um => um.IsEmailConfirmedAsync(user))
                .ReturnsAsync(true);
            Task<string> validToken = null;
            jwtFactoryMock.Setup(jf => jf.GeneratedEncodedTokenAsync(user))
                .Returns(validToken);

            // Act
            var result = await controller.Login(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = okResult.Value as string;
            Assert.Equal("validToken", token);
        }
    }
}


