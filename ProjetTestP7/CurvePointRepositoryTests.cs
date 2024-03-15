using System;
using Xunit;
using Moq;
using NouveauP7API.Repositories;
using NouveauP7API.Domain;
using System.Threading.Tasks;

namespace NouveauP7API.Tests.Repositories
{
    public class CurvePointRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsCurvePoint()
        {
            // Arrange
            int id = 1; // Spécifiez l'ID valide d'un point de courbe existant
            var expectedCurvePoint = new CurvePoints(); // Définir ici le point de courbe attendu avec l'ID spécifié
            var repositoryMock = new Mock<ICurvePointRepository>();
            repositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedCurvePoint);
            var repository = repositoryMock.Object;

            // Act
            var result = await repository.GetByIdAsync(id);

            // Assert
            Assert.Equal(expectedCurvePoint, result);
        }

        [Fact]
        public async Task AddAsync_ValidCurvePoint_AddsToList()
        {
            // Arrange
            var curvePointToAdd = new CurvePoints(); // Créez ici un point de courbe valide à ajouter
            var repositoryMock = new Mock<ICurvePointRepository>();
            repositoryMock.Setup(repo => repo.AddAsync(curvePointToAdd)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.AddAsync(curvePointToAdd);

            // Assert
            repositoryMock.Verify(repo => repo.AddAsync(curvePointToAdd), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidCurvePoint_UpdatesList()
        {
            // Arrange
            var curvePointToUpdate = new CurvePoints(); // Créez ici un point de courbe valide à mettre à jour
            var repositoryMock = new Mock<ICurvePointRepository>();
            repositoryMock.Setup(repo => repo.UpdateAsync(curvePointToUpdate)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.UpdateAsync(curvePointToUpdate);

            // Assert
            repositoryMock.Verify(repo => repo.UpdateAsync(curvePointToUpdate), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_DeletesCurvePoint()
        {
            // Arrange
            int idToDelete = 1; // Spécifiez l'ID valide du point de courbe à supprimer
            var repositoryMock = new Mock<ICurvePointRepository>();
            repositoryMock.Setup(repo => repo.DeleteAsync(idToDelete)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.DeleteAsync(idToDelete);

            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }
    }
}
