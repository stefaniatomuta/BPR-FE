using AutoMapper;
using BPR.Mediator.Interfaces;
using BPR.Mediator.Services;
using BPR.Mediator.Utils;
using BPR.Mediator.Validators;
using BPR.Model.Architectures;
using Microsoft.Extensions.Logging;
using NSubstitute.ReturnsExtensions;

namespace BPR.Tests;

[TestFixture]
internal class DependencyServiceTests
{
    private IDependencyService uut;
    private IDependencyRepository repositoryStub;
    private IValidatorService validatorStub;
    private ILogger<DependencyService> _logger;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        repositoryStub = Substitute.For<IDependencyRepository>();
        validatorStub = Substitute.For<IValidatorService>();
        _logger = Substitute.For<ILogger<DependencyService>>();
        uut = new DependencyService(repositoryStub, validatorStub, _logger);
    }

    [Test]
    public async Task AddModelAsync_WhenModelIsSuccessfullyAdded_ReturnsOkResult()
    {
        // Arrange
        var okResult = Result.Ok(_logger, "");
        repositoryStub.AddModelAsync(Arg.Any<ArchitecturalModel>()).Returns(okResult);
        validatorStub.ValidateArchitecturalModelAsync(Arg.Any<ArchitecturalModel>()).Returns(okResult);

        // Act
        var result = await uut.AddOrEditModelAsync(new ArchitecturalModel());

        // Assert
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task AddModelAsync_WhenValidationFails_ReturnsFailResult()
    {
        // Arrange
        validatorStub.ValidateArchitecturalModelAsync(Arg.Any<ArchitecturalModel>()).Returns(Result.Fail());

        // Act
        var result = await uut.AddOrEditModelAsync(new ArchitecturalModel());

        // Assert
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public async Task DeleteArchitectureModelAsync_WhenModelExists_ReturnsOkResult()
    {
        // Arrange
        repositoryStub.DeleteModelAsync(Arg.Any<Guid>()).Returns(new ArchitecturalModel());
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