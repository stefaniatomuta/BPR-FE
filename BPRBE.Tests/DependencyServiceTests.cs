using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using BPRBE.Services;
using NSubstitute.ReturnsExtensions;

namespace BPRBE.Tests;

[TestFixture]
internal class DependencyServiceTests
{
    private IDependencyService uut;
    private IDependencyRepository repositoryStub;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        repositoryStub = Substitute.For<IDependencyRepository>();
        uut = new DependencyService(repositoryStub);
    }

    [Test]
    public async Task DeleteArchitectureModelAsync_WhenModelExists_ReturnsOkResult()
    {
        // Arrange
        repositoryStub.DeleteModelAsync(Arg.Any<int>()).Returns(new ArchitecturalModel());
        var modelId = 1;

        // Act
        var result = await uut.DeleteArchitectureModelAsync(modelId);

        // Assert
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task DeleteArchitectureModelAsync_WhenModelDoesNotExist_ReturnsFailResult()
    {
        // Arrange
        repositoryStub.DeleteModelAsync(Arg.Any<int>()).ReturnsNull();
        var modelId = 1;

        // Act
        var result = await uut.DeleteArchitectureModelAsync(modelId);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }
}
