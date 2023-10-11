﻿using BPR.Persistence.Config;
using BPR.Persistence.Models;
using BPR.Persistence.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
            var result = await GetArchitecturalModelByName(modelCollection);
            if (result != null) return Result.Fail<ArchitecturalModelCollection>("Model with the same name already exists");
            await _dependenciesRuleCollection.InsertOneAsync(modelCollection);
            _logger.LogInformation("Architectural modelCollection added" + modelCollection);
            return Result.Ok(modelCollection);
        }
        catch (Exception e)
        {
            var message = e.Message;
            _logger.LogError(message);
            return Result.Fail<ArchitecturalModelCollection>(message);
        }
    }

    public async Task<ArchitecturalModelCollection?> DeleteModelAsync(ObjectId id)
    {
        var filter = Builders<ArchitecturalModelCollection>.Filter.Eq(model => model.Id, id);
        return await _dependenciesRuleCollection.FindOneAndDeleteAsync(filter);
    }

    private async Task<ArchitecturalModelCollection?> GetArchitecturalModelByName(ArchitecturalModelCollection modelCollection)
    {
        return await (await _dependenciesRuleCollection.FindAsync(x => x.Name.Equals(modelCollection.Name))).FirstOrDefaultAsync();
    }
}