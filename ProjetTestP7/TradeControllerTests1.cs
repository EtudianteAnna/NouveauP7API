using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NouveauP7API.Domain;
using NouveauP7API.Repositories;
using Xunit;

namespace NouveauP7API.Tests.Controllers
{
    public class TradeControllerTests
    {
        // Test method for the Post action
        [Fact]
        public async Task Post_ValidTrade_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var expectedTrade = new Trade { /* Initialize with valid properties */ };
            var mockRepository = new Mock<ITradeRepository>();
            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Trade>())).Returns(Task.CompletedTask);
            var controller = new TradeController(Mock.Of<ILogger<TradeController>>(), mockRepository.Object);

            // Act
            var result = await controller.Post(expectedTrade);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.NotNull(@object: createdAtActionResult.RouteValues["id"]);
            Assert.Same(expectedTrade, createdAtActionResult.Value);
        }

        // Test method for the Put action
        [Fact]
        public async Task Put_ValidIdAndTrade_ReturnsNoContentResult()
        {
            // Arrange
            var tradeId = 1;
            var tradeToUpdate = new Trade { TradeId = tradeId, /* Initialize with updated properties */ };
            var mockRepository = new Mock<ITradeRepository>();
            mockRepository.Setup(repo => repo.UpdateAsync(tradeToUpdate)).Returns(Task.CompletedTask);
            var controller = new TradeController(Mock.Of<ILogger<TradeController>>(), mockRepository.Object);

            // Act
            var result = await controller.Put(tradeId, tradeToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test method for the Delete action
        [Fact]
        public async Task Delete_ValidId_ReturnsNoContentResult()
        {
            // Arrange
            var tradeId = 1;
            var mockRepository = new Mock<ITradeRepository>();
            mockRepository.Setup(repo => repo.DeleteAsync(tradeId)).Returns(Task.CompletedTask);
            var controller = new TradeController(Mock.Of<ILogger<TradeController>>(), mockRepository.Object);

            // Act
            var result = await controller.Delete(tradeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

