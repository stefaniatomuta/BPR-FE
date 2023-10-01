using BPR.Models.Persistence;
using BPRBE.Config;
using BPRBE.Constants;
using BPRBE.Models.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BPRBE.Persistence;

public class DependencyRepository : IDependencyRepository
{
    private readonly IMongoCollection<MongoArchitecturalModel> _dependenciesRuleCollection;
    private readonly IMongoCollection<BsonDocument> _sequenceCollection;

    public DependencyRepository(IOptions<DatabaseConfig> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _dependenciesRuleCollection =
            mongoDatabase.GetCollection<MongoArchitecturalModel>(databaseSettings.Value.DependenciesRuleCollectionName);
        _sequenceCollection = mongoDatabase.GetCollection<BsonDocument>(databaseSettings.Value.DependenciesRuleCollectionName);
    }

    public async Task<IList<MongoArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        return await (await _dependenciesRuleCollection.FindAsync(_ => true)).ToListAsync();
    }
    
    public async Task<MongoArchitecturalModel?> GetArchitecturalModelByName(MongoArchitecturalModel model)
    {
        return await (await _dependenciesRuleCollection.FindAsync(x => x.Name.Equals(model.Name))).FirstOrDefaultAsync();
    }
    
    public async Task<Result> AddModelAsync(MongoArchitecturalModel model)
    {
        try
        {
            var result = await GetArchitecturalModelByName(model);
            if (result != null) return Result.Fail<MongoArchitecturalModel>("Model with the same name already exists");
            model.Id = await GetNextSequenceValueAsync();
            await _dependenciesRuleCollection.InsertOneAsync(model);
            return Result.Ok(model);
        }
        catch (Exception e)
        {
            return Result.Fail<MongoArchitecturalModel>(e.Message);
        }
    }
    
    private async Task<int> GetNextSequenceValueAsync()
    {
        var filter = Builders<BsonDocument>.Filter.Eq(Values.MongoDbId, Values.MongoDbId);
        var update = Builders<BsonDocument>.Update.Inc(Values.MongoDbValue, 1);
        var options = new FindOneAndUpdateOptions<BsonDocument, BsonDocument> { IsUpsert = true, ReturnDocument = ReturnDocument.After };
        var result = await _sequenceCollection.FindOneAndUpdateAsync(filter, update, options);

        return result[Values.MongoDbValue].AsInt32;
    }
}