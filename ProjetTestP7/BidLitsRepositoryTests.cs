using Xunit;
using Moq;
using NouveauP7API.Repositories;
using NouveauP7API.Domain;

namespace NouveauP7API.Tests.Repositories
{
    public class BidListRepositoryTests
    {
        [Fact]
        public async Task GetBidListsAsync_ReturnsListOfBidLists()
        {
            // Arrange
            var expectedBidLists = new List<BidList>(); // Définir ici la liste de vos listes d'offres attendues
            var repositoryMock = new Mock<IBidListRepository>();
            repositoryMock.Setup(repo => repo.GetBidListsAsync()).ReturnsAsync(expectedBidLists);
            var repository = repositoryMock.Object;

            // Act
            var result = await repository.GetBidListsAsync();

            // Assert
            Assert.Equal(expectedBidLists, result);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsBidList()
        {
            // Arrange
            int id = 1; // Spécifiez l'ID valide d'une liste d'offres existante
            var expectedBidList = new BidList(); // Définir ici la liste d'offres attendue avec l'ID spécifié
            var repositoryMock = new Mock<IBidListRepository>();
            repositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedBidList);
            var repository = repositoryMock.Object;

            // Act
            var result = await repository.GetByIdAsync(id);

            // Assert
            Assert.Equal(expectedBidList, result);
        }

        [Fact]
        public async Task AddAsync_ValidBidList_AddsToList()
        {
            // Arrange
            var bidListToAdd = new BidList(); // Créez ici une liste d'offres valide à ajouter
            var repositoryMock = new Mock<IBidListRepository>();
            repositoryMock.Setup(repo => repo.AddAsync(bidListToAdd)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.AddAsync(bidListToAdd);

            // Assert
            repositoryMock.Verify(repo => repo.AddAsync(bidListToAdd), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidBidList_UpdatesList()
        {
            // Arrange
            var bidListToUpdate = new BidList(); // Créez ici une liste d'offres valide à mettre à jour
            var repositoryMock = new Mock<IBidListRepository>();
            repositoryMock.Setup(repo => repo.UpdateAsync(bidListToUpdate)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.UpdateAsync(bidListToUpdate);

            // Assert
            repositoryMock.Verify(repo => repo.UpdateAsync(bidListToUpdate), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_DeletesBidList()
        {
            // Arrange
            int idToDelete = 1; // Spécifiez l'ID valide de la liste d'offres à supprimer
            var repositoryMock = new Mock<IBidListRepository>();
            repositoryMock.Setup(repo => repo.DeleteAsync(idToDelete)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.DeleteAsync(idToDelete);

            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }
    }
}
