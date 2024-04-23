using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using Xunit;

namespace NouveauP7API.Tests.Controllers
{
    public class UserControllerTests
    {
        // Test method for the AddUser action
        [Fact]
        public async Task AddUser_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var expectedUser = new User { /* Initialize with valid properties */ };
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            var controller = new UserController(Mock.Of<ILogger<UserController>>(), mockRepository.Object);

            // Act
            var result = await controller.AddUser(expectedUser);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        // Test method for the Validate action
        [Fact]
        public async Task Validate_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var expectedUser = new User { /* Initialize with valid properties */ };
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            var controller = new UserController(Mock.Of<ILogger<UserController>>(), mockRepository.Object);

            // Act
            var result = await controller.Validate(expectedUser);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        // Test method for the DeleteUser action
        [Fact]
        public async Task DeleteUser_ValidId_ReturnsOkResult()
        {
            // Arrange
            var userId = "1";
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.DeleteAsync(userId)).Returns(Task.CompletedTask);
            var controller = new UserController(Mock.Of<ILogger<UserController>>(), mockRepository.Object);

            // Act
            var result = await controller.DeleteUser(userId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        // Test method for the UpdateUser action
        [Fact]
        public async Task UpdateUser_ValidIdAndUser_ReturnsOkResult()
        {
            // Arrange
            var userId = "1";
            var existingUser = new User { /* Initialize with existing user properties */ };
            var updatedUser = new User { /* Initialize with updated user properties */ };
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            mockRepository.Setup(repo => repo.UpdateAsync(existingUser)).Returns(Task.CompletedTask);
            var controller = new UserController(Mock.Of<ILogger<UserController>>(), mockRepository.Object);

            // Act
            var result = await controller.UpdateUser(userId, updatedUser);

            // Assert
            Assert.IsType<OkResult>(result);
        }

       
    }
}
