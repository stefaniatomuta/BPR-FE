using BPR.Mediator.Interfaces;
using BPR.Mediator.Services;
using BPR.Mediator.Utils;
using BPR.Mediator.Validators;
using BPR.Model.Architectures;
using Microsoft.Extensions.Logging;
using NSubstitute.ReturnsExtensions;

namespace BPR.Tests;

[TestFixture]
internal class ArchitectureModelServiceTests
{
    private IArchitectureModelService uut = default!;
    private IArchitectureModelRepository repositoryStub = default!;
    private IValidatorService validatorStub = default!;
    private ILogger<ArchitectureModelService> _logger = default!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        repositoryStub = Substitute.For<IArchitectureModelRepository>();
        validatorStub = Substitute.For<IValidatorService>();
        _logger = Substitute.For<ILogger<ArchitectureModelService>>();
        uut = new ArchitectureModelService(repositoryStub, validatorStub, _logger);
    }

    [Test]
    public async Task AddModelAsync_WhenModelIsSuccessfullyAdded_ReturnsOkResult()
    {
        // Arrange
        var okResult = Result.Ok(_logger, "");
        repositoryStub.AddModelAsync(Arg.Any<ArchitectureModel>()).Returns(okResult);
        validatorStub.ValidateArchitectureModelAsync(Arg.Any<ArchitectureModel>()).Returns(okResult);

        // Act
        var result = await uut.AddOrEditModelAsync(new ArchitectureModel());

        // Assert
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task AddModelAsync_WhenValidationFails_ReturnsFailResult()
    {
        // Arrange
        validatorStub.ValidateArchitectureModelAsync(Arg.Any<ArchitectureModel>()).Returns(Result.Fail());

        // Act
        var result = await uut.AddOrEditModelAsync(new ArchitectureModel());

        // Assert
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public async Task DeleteArchitectureModelAsync_WhenModelExists_ReturnsOkResult()
    {
        // Arrange
        repositoryStub.DeleteModelAsync(Arg.Any<Guid>()).Returns(new ArchitectureModel());
        var modelId = new Guid();

        // Act
        var result = await uut.DeleteArchitectureModelAsync(modelId);

        // Assert
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task DeleteArchitectureModelAsync_WhenModelDoesNotExist_ReturnsFailResult()
    {
        // Arrange
        repositoryStub.DeleteModelAsync(Arg.Any<Guid>()).ReturnsNull();
        var modelId = new Guid();

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