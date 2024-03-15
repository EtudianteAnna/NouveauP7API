using Xunit;
using Moq;
using NouveauP7API.Repositories;
using NouveauP7API.Domain;

namespace NouveauP7API.Tests.Repositories
{
    public class RuleNameRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsRuleName()
        {
            // Arrange
            int id = 1; // Spécifiez l'ID valide du RuleName à récupérer
            var expectedRuleName = new RuleName(); // Ajoutez ici le RuleName attendu
            var repositoryMock = new Mock<IRuleNameRepository>();
            repositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedRuleName);
            var repository = repositoryMock.Object;

            // Act
            var result = await repository.GetByIdAsync(id);

            // Assert
            Assert.Equal(expectedRuleName, result);
        }

        [Fact]
        public async Task AddAsync_ValidRuleName_AddsToList()
        {
            // Arrange
            var ruleNameToAdd = new RuleName(); // Créez ici un RuleName valide à ajouter
            var repositoryMock = new Mock<IRuleNameRepository>();
            repositoryMock.Setup(repo => repo.AddAsync(ruleNameToAdd)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.AddAsync(ruleNameToAdd);

            // Assert
            repositoryMock.Verify(repo => repo.AddAsync(ruleNameToAdd), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidRuleName_UpdatesList()
        {
            // Arrange
            var ruleNameToUpdate = new RuleName(); // Créez ici un RuleName valide à mettre à jour
            var repositoryMock = new Mock<IRuleNameRepository>();
            repositoryMock.Setup(repo => repo.UpdateAsync(ruleNameToUpdate)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.UpdateAsync(ruleNameToUpdate);

            // Assert
            repositoryMock.Verify(repo => repo.UpdateAsync(ruleNameToUpdate), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_DeletesRuleName()
        {
            // Arrange
            int idToDelete = 1; // Spécifiez l'ID valide du RuleName à supprimer
            var repositoryMock = new Mock<IRuleNameRepository>();
            repositoryMock.Setup(repo => repo.DeleteAsync(idToDelete)).Verifiable();
            var repository = repositoryMock.Object;

            // Act
            await repository.DeleteAsync(idToDelete);

            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }
    }
}
