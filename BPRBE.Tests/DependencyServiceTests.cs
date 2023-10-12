using AutoMapper;
using BPR.Persistence.Models;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using BPRBE.Services.Models;
using BPRBE.Services.Services;
using BPRBE.Services.Validators;
using Microsoft.Extensions.Logging;
using NSubstitute.ReturnsExtensions;

namespace BPRBE.Tests;

[TestFixture]
internal class DependencyServiceTests
{
    private IDependencyService uut;
    private IDependencyRepository repositoryStub;
    private IValidatorService validatorStub;
    private IMapper _mapper;
    private ILogger<DependencyService> _logger;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        repositoryStub = Substitute.For<IDependencyRepository>();
        validatorStub = Substitute.For<IValidatorService>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ArchitecturalModel, ArchitecturalModelCollection>().ReverseMap();
        });
        _mapper = new Mapper(mapperConfig);
        _logger = Substitute.For<ILogger<DependencyService>>();
        uut = new DependencyService(repositoryStub, _mapper, validatorStub, _logger);
    }

    [Test]
    public async Task AddModelAsync_WhenModelIsSuccessfullyAdded_ReturnsOkResult()
    {
        // Arrange
        var okResult = Result.Ok(_logger, "");
        repositoryStub.AddModelAsync(Arg.Any<ArchitecturalModelCollection>()).Returns(okResult);
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
        repositoryStub.DeleteModelAsync(Arg.Any<Guid>()).Returns(new ArchitecturalModelCollection());
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
