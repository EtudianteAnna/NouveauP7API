using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NouveauP7API.Domain;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using Xunit;

namespace ProjetTestP7
{
    public class AuthentificationControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtFactory> _jwtFactoryMock;
        private readonly AuthentificationController _controller;
        public AuthentificationControllerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtFactoryMock = new Mock<IJwtFactory>();
            _controller = new AuthentificationController(_userRepositoryMock.Object, _jwtFactoryMock.Object);
        }


        //Scénario de test pour la création d'un nouvel utilisateur et la génération du jeton JWT
        [Fact]
        public async Task LoginNewUserCreatesAndReturnsToken()
        {
            // Arrange
            var model = new LoginModel { Username = "testuser", Email = "test@example.com", Password = "password" };
            var token = "testToken";

            // Configuration du comportement du repository
            _userRepositoryMock.Setup(repo => repo.GetUserByCredentialsAsync(model.Username))
                .ReturnsAsync(new User());

            _jwtFactoryMock.Setup(factory => factory.GeneratedEncodedToken(It.IsAny<(string, string, string)>()))
                .Returns(token);

            // Act
            var result = await _controller.Login(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            var responseData = result.Value as dynamic;
            Assert.NotNull(responseData);
            Assert.Equal("Utilisateur créé et connecté avec succès", responseData.Message);
            Assert.Equal(token, responseData.Token);

            // Vérification des appels aux méthodes des dépendances simulées
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<(string, string, string)>()), Times.Once);
            _jwtFactoryMock.Verify(factory => factory.GeneratedEncodedToken(It.IsAny<(string, string, string)>()), Times.Once);
        }

        //Scénario de test pour la connexion d'un utilisateur existant et la génération du jeton JWT :
        [Fact]
        public async Task LoginExistingUserReturnsToken()
        {
            // Arrange
            var model = new LoginModel { Username = "existinguser", Password = "password" };
            var user = new User { UserName = "existinguser" };
            var token = "testToken";

            // Configuration du comportement simulé des dépendances
            _userRepositoryMock.Setup(repo => repo.GetUserByCredentialsAsync(model.Username))
                .ReturnsAsync(user);

            if (user != null)
                _jwtFactoryMock.Setup(factory => factory.GeneratedEncodedToken(It.IsAny<User>()))
                .Returns(token);

            // Act
            var result = await _controller.Login(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            var responseData = result.Value as dynamic;
            Assert.NotNull(responseData);
            Assert.Equal("Connexion réussie", responseData.Message);
            Assert.Equal(token, responseData.Token);

            // Vérification des appels aux méthodes des dépendances simulées
            _userRepositoryMock.Verify(repo => repo.GetUserByCredentialsAsync(model.Username), Times.Once);
            _jwtFactoryMock.Verify(factory => factory.GeneratedEncodedToken(It.IsAny<User>()), Times.Once);
        }

        //Scénario de test pour la validation du modèle lorsqu'il est invalide :
        [Fact]
        public async Task Login_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new LoginModel();
            _controller.ModelState.AddModelError("Username", "Le nom d'utilisateur est requis");

            // Act
            var result = await _controller.Login(model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

            // Vérification des appels aux méthodes des dépendances simulées
            _userRepositoryMock.Verify(repo => repo.GetUserByCredentialsAsync(It.IsAny<string>()), Times.Never);
            _jwtFactoryMock.Verify(factory => factory.GeneratedEncodedToken(It.IsAny<User>()), Times.Never);
        }




    }

}