using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NouveauP7API.Controllers;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using System.Security.Claims;
using Xunit;

public class AuthentificationControllerTests
{
   
    [Fact]
    public async Task Login_WithInvalidUsername_ReturnsUnauthorizedResult()
    {
        // Arrange
        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        var roleManager = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        var jwtFactory = new Mock<IJwtFactory>();

        var controller = new AuthentificationController(userManager.Object, roleManager.Object, jwtFactory.Object);

        var loginModel = new LoginModel
        {
            Username = "invalidUsername",
            Password = "validPassword"
        };

        userManager.Setup(um => um.FindByNameAsync(loginModel.Username))
            .ReturnsAsync((User)null);

        // Act
        var result = await controller.Login(loginModel);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }
    /*[Fact]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        var roleManager = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        var jwtFactory = new Mock<IJwtFactory>();

        var controller = new AuthentificationController(userManager.Object, roleManager.Object, jwtFactory.Object);

        var loginModel = new LoginModel
        {
            Username = "validUsername",
            Password = "validPassword"
        };

        var user = new User
        {
            UserName = loginModel.Username,
            Email = "valid@example.com",
            EmailConfirmed = true
        };

        userManager.Setup(um => um.FindByNameAsync(loginModel.Username))
            .ReturnsAsync(user);
        userManager.Setup(um => um.CheckPasswordAsync(user, loginModel.Password))
            .ReturnsAsync(true);
        userManager.Setup(um => um.IsEmailConfirmedAsync(user))
            .ReturnsAsync(true);
        jwtFactory.Setup(jf => jf.GeneratedEncodedTokenAsync(It.IsAny<List<Claim>>()))
            .ReturnsAsync("validToken");

        // Act
        var result = await controller.Login(loginModel);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("validToken", ((OkObjectResult)result).Value);
    }*/
    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorizedResult()
    {
        // Arrange
        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        var roleManager = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        var jwtFactory = new Mock<IJwtFactory>();

        var controller = new AuthentificationController(userManager.Object, roleManager.Object, jwtFactory.Object);

        var loginModel = new LoginModel
        {
            Username = "validUsername",
            Password = "invalidPassword"
        };

        var user = new User
        {
            UserName = loginModel.Username,
            Email = "valid@example.com",
            EmailConfirmed = true
        };

        userManager.Setup(um => um.FindByNameAsync(loginModel.Username))
            .ReturnsAsync(user);
        userManager.Setup(um => um.CheckPasswordAsync(user, loginModel.Password))
            .ReturnsAsync(false);

        // Act
        var result = await controller.Login(loginModel);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Login_WithUnconfirmedEmail_ReturnsBadRequestResult()
    {
        // Arrange
        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        var roleManager = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        var jwtFactory = new Mock<IJwtFactory>();

        var controller = new AuthentificationController(userManager.Object, roleManager.Object, jwtFactory.Object);

        var loginModel = new LoginModel
        {
            Username = "validUsername",
            Password = "validPassword"
        };

        var user = new User
        {
            UserName = loginModel.Username,
            Email = "valid@example.com",
            EmailConfirmed = false
        };

        userManager.Setup(um => um.FindByNameAsync(loginModel.Username))
            .ReturnsAsync(user);
        userManager.Setup(um => um.CheckPasswordAsync(user, loginModel.Password))
            .ReturnsAsync(true);
        userManager.Setup(um => um.IsEmailConfirmedAsync(user))
            .ReturnsAsync(false);

        // Act
        var result = await controller.Login(loginModel);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("L'adresse email n'est pas confirmée.", badRequestResult.Value);
    }
}