using BPRBE.Config;
using BPRBE.Models.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BPRBE.Persistence;

public class MongoDependencyRepository : IMongoDependencyRepository
{
    private readonly IMongoCollection<ArchitecturalModel> _dependenciesRuleCollection;

    public MongoDependencyRepository(IOptions<DatabaseConfig> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _dependenciesRuleCollection =
            mongoDatabase.GetCollection<ArchitecturalModel>(databaseSettings.Value.DependenciesRuleCollectionName);
    }

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        return await (await _dependenciesRuleCollection.FindAsync(_ => true)).ToListAsync();
    }
    public async Task<ArchitecturalModel?> GetArchitecturalModelByName(ArchitecturalModel model)
    {
        return await (await _dependenciesRuleCollection.FindAsync(x => x.Name.Equals(model.Name))).FirstOrDefaultAsync();
    }
    public async Task AddModelAsync(ArchitecturalModel model)
    {
        var result = await GetArchitecturalModelByName(model);
        if (result == null)
        {
            await _dependenciesRuleCollection.InsertOneAsync(model);
        }
    }
}