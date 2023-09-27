using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BPRBE.Config;
using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using NSubstitute;
using NUnit.Framework;

namespace BPRBE.Tests;

[TestFixture]
public class DependencyRepositoryTests
{
    private IDependencyRepository _repository;
    private IMongoCollection<ArchitecturalModel> _modelCollection;
    private IOptions<DatabaseConfig> _optionsMock;

    [SetUp]
    public void Setup()
    {
        _optionsMock = Substitute.For<IOptions<DatabaseConfig>>();

        var settings = new MongoClientSettings
        {
            Server = new MongoServerAddress("localhost", 27017)
        };

        var mongoClient = new MongoClient(settings);
        var database = mongoClient.GetDatabase("testdb");

        _modelCollection = database.GetCollection<ArchitecturalModel>("Dependencies");

        _optionsMock.Value.Returns(new DatabaseConfig
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "testdb",
            DependenciesRuleCollectionName = "architecturalModels"
        });

        _repository = new DependencyRepository(_optionsMock);
    }


    [Test]
    public async Task GetArchitecturalModelByName_ShouldReturnModel()
    {
        // Arrange
        var model = new ArchitecturalModel { Name = "NewModel" };
        _repository.GetArchitecturalModelByName(Arg.Any<ArchitecturalModel>())
            .Returns(x => null as ArchitecturalModel);

        // _repository.GetArchitecturalModelByName(model).ReturnsForAnyArgs(null as ArchitecturalModel);
        var collectionSubstitute = Substitute.For<IMongoCollection<ArchitecturalModel>>();

        _modelCollection.Returns(collectionSubstitute);

        // Act
        var result = await _repository.AddModelAsync(model);

        // Assert
        await collectionSubstitute.Received(1).InsertOneAsync(model, null, default);
    }
}