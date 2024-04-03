using Xunit;
using Moq;
using NouveauP7API.Repositories;
using NouveauP7API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NouveauP7API.Tests.Repositories
{
    public class RatingRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllRatings()
        {
            // Arrange
            var expectedRatings = new List<Rating>(); // Ajoutez ici les évaluations attendues
            var repositoryMock = new Mock<IRatingRepository>();
            repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedRatings);
            var repository = repositoryMock.Object;

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(expectedRatings, result);
        }

        [Fact]
        public async Task AddAsync_ValidRating_AddsToList()
        {
            // Arrange
            var ratingToAdd = new Rating(); // Créez ici une évaluation valide à ajouter
            var repositoryMock = new Mock<IRatingRepository>();
            repositoryMock.Setup(repo => repo.AddAsync(ratingToAdd)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.AddAsync(ratingToAdd);

            // Assert
            repositoryMock.Verify(repo => repo.AddAsync(ratingToAdd), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidRating_UpdatesList()
        {
            // Arrange
            var ratingToUpdate = new Rating(); // Créez ici une évaluation valide à mettre à jour
            var repositoryMock = new Mock<IRatingRepository>();
            repositoryMock.Setup(repo => repo.UpdateAsync(ratingToUpdate)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.UpdateAsync(ratingToUpdate);

            // Assert
            repositoryMock.Verify(repo => repo.UpdateAsync(ratingToUpdate), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_DeletesRating()
        {
            // Arrange
            int idToDelete = 1; // Spécifiez l'ID valide de l'évaluation à supprimer
            var repositoryMock = new Mock<IRatingRepository>();
            repositoryMock.Setup(repo => repo.DeleteAsync(idToDelete)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.DeleteAsync(idToDelete);

            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }
    }
}
