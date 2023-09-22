using BPRBE.Config;
using BPRBE.Constants;
using BPRBE.Models.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BPRBE.Persistence;

public class MongoDependencyRepository : IMongoDependencyRepository
{
    private readonly IMongoCollection<ArchitecturalModel> _dependenciesRuleCollection;
    private readonly IMongoCollection<BsonDocument> _sequenceCollection;

    public MongoDependencyRepository(IOptions<DatabaseConfig> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _dependenciesRuleCollection =
            mongoDatabase.GetCollection<ArchitecturalModel>(databaseSettings.Value.DependenciesRuleCollectionName);
        _sequenceCollection = mongoDatabase.GetCollection<BsonDocument>(databaseSettings.Value.DependenciesRuleCollectionName);
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
            model.Id =  await GetNextSequenceValueAsync();
            await _dependenciesRuleCollection.InsertOneAsync(model);
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