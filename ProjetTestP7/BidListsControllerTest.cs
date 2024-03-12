using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using NouveauP7API.Repositories;
using NouveauP7API.Controllers;
using NouveauP7API.Domain;
    


namespace NouveauP7API;

public class BidListsControllerTests
{
    [Fact]
    public async Task Get_ReturnsOkResult_WhenBidListExists()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<BidListsController>>();
        var repositoryMock = new Mock<IBidListRepository>();
        var bidList = new BidList { BidListId = 42 }; // Example bidList with ID 42
        repositoryMock.Setup(repo => repo.GetByIdAsync(42)).ReturnsAsync(bidList);
        var controller = new BidListsController(loggerMock.Object, repositoryMock.Object);

        // Act
        var result = await controller.Get(42);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedBidList = Assert.IsType<BidList>(okResult.Value);
        Assert.Equal(42, returnedBidList.BidListId);
        Assert.Equal(bidList, returnedBidList);
    }
}
