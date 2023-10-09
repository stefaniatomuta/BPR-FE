using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using BPRBE.Services;
using BPRBE.Validators;
using MongoDB.Bson;
using NSubstitute.ReturnsExtensions;

namespace BPRBE.Tests;

[TestFixture]
internal class DependencyServiceTests
{
    private IDependencyService uut;
    private IDependencyRepository repositoryStub;
    private IValidatorService validatorStub;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        repositoryStub = Substitute.For<IDependencyRepository>();
        validatorStub = Substitute.For<IValidatorService>();
        uut = new DependencyService(repositoryStub, validatorStub);
    }

    [Test]
    public async Task DeleteArchitectureModelAsync_WhenModelExists_ReturnsOkResult()
    {
        // Arrange
        repositoryStub.DeleteModelAsync(Arg.Any<ObjectId>()).Returns(new ArchitecturalModel());
        var modelId = ObjectId.GenerateNewId();

        // Act
        var result = await uut.DeleteArchitectureModelAsync(modelId);

        // Assert
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task DeleteArchitectureModelAsync_WhenModelDoesNotExist_ReturnsFailResult()
    {
        // Arrange
        repositoryStub.DeleteModelAsync(Arg.Any<ObjectId>()).ReturnsNull();
        var modelId = ObjectId.GenerateNewId();

        // Act
        var result = await uut.DeleteArchitectureModelAsync(modelId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
        });
    }
}
