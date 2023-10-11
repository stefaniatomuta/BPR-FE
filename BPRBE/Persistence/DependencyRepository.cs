using BPRBE.Config;
using BPRBE.Models.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BPRBE.Persistence;

public class DependencyRepository : IDependencyRepository
{
    private readonly IMongoCollection<ArchitecturalModel> _dependenciesRuleCollection;

    public DependencyRepository(IOptions<DatabaseConfig> databaseSettings)
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

    public async Task<Result> AddModelAsync(ArchitecturalModel model)
    {
        try
        {
            var result = await GetArchitecturalModelByName(model);
            if (result != null) return Result.Fail<ArchitecturalModel>("Model with the same name already exists");
            await _dependenciesRuleCollection.InsertOneAsync(model);
            return Result.Ok(model);
        }
        catch (Exception e)
        {
            return Result.Fail<ArchitecturalModel>(e.Message);
        }
    }

    public async Task<ArchitecturalModel?> DeleteModelAsync(ObjectId id)
    {
        var filter = Builders<ArchitecturalModel>.Filter.Eq(model => model.Id, id);
        return await _dependenciesRuleCollection.FindOneAndDeleteAsync(filter);
    }

    private async Task<ArchitecturalModel?> GetArchitecturalModelByName(ArchitecturalModel model)
    {
        return await (await _dependenciesRuleCollection.FindAsync(x => x.Name.Equals(model.Name))).FirstOrDefaultAsync();
    }
}