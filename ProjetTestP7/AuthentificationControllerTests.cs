
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NouveauP7API.Controllers;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using Xunit;

public class AuthentificationControllerTests

{
    [Fact]
    public async Task LoginWithValidCredentialsReturnsOkResult()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your_secret_key_here",
            Issuer = "your_issuer_here",
            Audience = "your_audience_here"
        };

        var userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);

        userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                EmailConfirmed = true
            });

        userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        userManagerMock.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        var jwtFactoryMock = new Mock<IJwtFactory>();
        jwtFactoryMock.Setup(jf => jf.GeneratedEncodedToken(It.IsAny<User>()))
            .Returns("valid_jwt_token");

        var controller = new AuthentificationController(null, userManagerMock.Object, jwtFactoryMock.Object);

        // Act
        var result = await controller.Login(new LoginModel
        {
            Username = "testuser",
            Password = "validpassword"
        });

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        var tokenResponse = Assert.IsAssignableFrom<TokenResponse>(okResult.Value);
        Assert.Equal("valid_jwt_token", tokenResponse.Token);
    }
    


    [Fact]
    public async Task LoginWithInvalidCredentialsReturnsUnauthorizedResult()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your_secret_key_here",
            Issuer = "your_issuer_here",
            Audience = "your_audience_here"
        };

        var userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);

        _ = userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(null as User);

        var jwtFactoryMock = new Mock<JwtFactory>(jwtSettings);

        var controller = new AuthentificationController(null, userManagerMock.Object, jwtFactoryMock.Object);

        // Act
        var result = await controller.Login(new LoginModel
        {
            Username = "invaliduser",
            Password = "invalidpassword"
        });

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task LoginWithUnconfirmedEmailReturnsBadRequestResult()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your_secret_key_here",
            Issuer = "your_issuer_here",
            Audience = "your_audience_here"
        };
        var jwtFactoryMock = new Mock<JwtFactory>(jwtSettings);
        jwtFactoryMock.Setup(jf => jf.GeneratedEncodedToken(It.IsAny<User>()))
            .Returns("valid_jwt_token");

        var userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);

        userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                EmailConfirmed = false
            });

        userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        userManagerMock.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<User>()))
            .ReturnsAsync(false);


        var controller = new AuthentificationController( null, userManagerMock.Object, jwtFactoryMock.Object);

        // Act
        var result = await controller.Login(new LoginModel
        {
            Username = "testuser",
            Password = "validpassword"
        });

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("L'adresse email n'est pas confirmée.", badRequestResult.Value);
    }
}



//Login avec des identifiants valides : vérifie que la méthode retourne un résultat de type OkObjectResult avec le jeton JWT attendu.
//Login avec des identifiants invalides : vérifie que la méthode retourne un résultat de type UnauthorizedResult.
//Login avec un email non confirmé : vérifie que la méthode retourne un résultat de type BadRequestObjectResult avec le message d'erreur attendu.
//Ces tests utilisent Moq pour créer des mocks des dépendances de la classe AuthentificationController,
//comme UserManager<User> et IJwtFactory. Cela permet de tester la méthode Login de manière isolée,
//sans dépendre des implémentations réelles de ces dépendances.