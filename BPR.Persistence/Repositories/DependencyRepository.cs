using BPR.Persistence.Config;
using BPR.Persistence.Models;
using BPR.Persistence.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BPR.Persistence.Repositories;

public class DependencyRepository : IDependencyRepository
{
    private readonly IMongoCollection<ArchitecturalModelCollection> _dependenciesRuleCollection;
    private readonly ILogger<DependencyRepository> _logger;

    public DependencyRepository(IOptions<DatabaseConfig> databaseSettings, ILogger<DependencyRepository> logger)
    {
        _logger = logger;
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _dependenciesRuleCollection =
            mongoDatabase.GetCollection<ArchitecturalModelCollection>(databaseSettings.Value.DependenciesRuleCollectionName);
    }

    public async Task<IList<ArchitecturalModelCollection>> GetArchitecturalModelsAsync()
    {
        return await (await _dependenciesRuleCollection.FindAsync(_ => true)).ToListAsync();
    }

    public async Task<Result> AddModelAsync(ArchitecturalModelCollection modelCollection)
    {
        try
        {
            var result = await GetArchitecturalModelCollectionByName(modelCollection);
            if (result != null) return Result.Fail<ArchitecturalModelCollection>("Model with the same name already exists", _logger);
            await _dependenciesRuleCollection.InsertOneAsync(modelCollection);
            _logger.LogInformation("Architectural modelCollection added" + modelCollection);
            return Result.Ok(modelCollection);
        }
        catch (Exception e)
        {
            var message = e.Message;
            _logger.LogError(message);
            return Result.Fail<ArchitecturalModelCollection>(message, _logger);
        }
    }
    
    public async Task<Result> EditModelAsync(ArchitecturalModelCollection model)
    {
        try
        {
            var result = await GetArchitecturalModelCollectionByName(model);
            if (result != null && result.Id != model.Id)
            {
                return Result.Fail<ArchitecturalModelCollection>("Model with the same name already exists", _logger);
            }

            var filter = Builders<ArchitecturalModelCollection>.Filter.Eq(old => old.Id, model.Id);
            var update = Builders<ArchitecturalModelCollection>.Update
                .Set(old => old.Name, model.Name)
                .Set(old => old.Components, model.Components);
            
            await _dependenciesRuleCollection.UpdateOneAsync(filter, update);

            return Result.Ok(model);
        }
        catch (Exception e)
        {
            return Result.Fail<ArchitecturalModelCollection>(e.Message, _logger);
        }
    }

    public async Task<ArchitecturalModelCollection?> DeleteModelAsync(Guid id)
    {
        var filter = Builders<ArchitecturalModelCollection>.Filter.Eq(model => model.Id, id);
        var result = await _dependenciesRuleCollection.FindOneAndDeleteAsync(filter);
        return result;
    }

    private async Task<ArchitecturalModelCollection?> GetArchitecturalModelCollectionByName(ArchitecturalModelCollection model)
    {
        return await (await _dependenciesRuleCollection.FindAsync(x => x.Name.Equals(model.Name))).FirstOrDefaultAsync();
    }
}