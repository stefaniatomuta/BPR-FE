using BPRBE.Config;
using BPRBE.Models.Persistence;
using FluentValidation;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BPRBE.Persistence;

public class MongoDependencyRepository : IMongoDependencyRepository
{
    private readonly IMongoCollection<ArchitecturalModel> _dependenciesRuleCollection;
    private readonly IValidator<ArchitecturalModel> _validator;

    public MongoDependencyRepository(IOptions<DatabaseConfig> databaseSettings, IValidator<ArchitecturalModel> validator)
    {
        _validator = validator;
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _dependenciesRuleCollection =
            mongoDatabase.GetCollection<ArchitecturalModel>(databaseSettings.Value.DependenciesRuleCollectionName);
    }

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        return await (await _dependenciesRuleCollection.FindAsync(_ => true)).ToListAsync();
    }
    public async Task AddModelAsync(ArchitecturalModel model)
    {
        await _validator.ValidateAndThrowAsync(model);
        await _dependenciesRuleCollection.InsertOneAsync(model);
    }
}