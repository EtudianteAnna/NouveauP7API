using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NouveauP7API.Controllers;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using Xunit;

namespace NouveauP7API.Tests.Controllers
{
    public class AuthentificationControllerTest
    {

        [Fact]
        public async Task Login_WithInvalidUsername_ReturnsUnauthorized()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var jwtFactoryMock = new Mock<IJwtFactory>();

            var controller = new AuthentificationController(userManagerMock.Object, jwtFactoryMock.Object);

            var loginModel = new LoginModel
            {
                Username = "invalidUsername",
                Password = "validPassword"
            };

            userManagerMock.Setup(um => um.FindByNameAsync(loginModel.Username))
                .ReturnsAsync((User)null);

            // Act
            var result = await controller.Login(loginModel);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var jwtFactoryMock = new Mock<IJwtFactory>();

            var controller = new AuthentificationController(userManagerMock.Object, jwtFactoryMock.Object);

            var loginModel = new LoginModel
            {
                Username = "validUsername",
                Password = "invalidPassword"
            };

            var user = new User
            {
                UserName = loginModel.Username,
                EmailConfirmed = true
            };

            userManagerMock.Setup(um => um.FindByNameAsync(loginModel.Username))
                .ReturnsAsync(user);
            userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginModel.Password))
                .ReturnsAsync(false);

            // Act
            var result = await controller.Login(loginModel);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_WithUnconfirmedEmail_ReturnsBadRequest()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
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
                EmailConfirmed = false
            };

            userManagerMock.Setup(um => um.FindByNameAsync(loginModel.Username))
                .ReturnsAsync(user);
            userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginModel.Password))
                .ReturnsAsync(true);
            userManagerMock.Setup(um => um.IsEmailConfirmedAsync(user))
                .ReturnsAsync(false);

            // Act
            var result = await controller.Login(loginModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("L'adresse email n'est pas confirmée.", badRequestResult.Value);
        }

        
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}