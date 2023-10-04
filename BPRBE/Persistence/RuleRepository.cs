using BPRBE.Config;
using BPRBE.Models.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BPRBE.Persistence;

public class RuleRepository : IRuleRepository
{
    private readonly IMongoCollection<Rule> _rulesCollection;
    
    public RuleRepository(IOptions<DatabaseConfig> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _rulesCollection = mongoDatabase.GetCollection<Rule>(databaseSettings.Value.RulesCollectionName);
    }
    
    public async Task<IList<Rule>> GetRulesAsync()
    {
        return await (await _rulesCollection.FindAsync(_ => true)).ToListAsync();
    }
    
    public async Task<Result> AddRuleAsync(Rule rule)
    {
        try
        {
            var result = await GetRulesAsync();
            if (result != null) return Result.Fail<Rule>("Rule with the same name already exists");
            
            await _rulesCollection.InsertOneAsync(rule);
            return Result.Ok(rule);
        }
        catch (Exception e)
        {
            return Result.Fail<Rule>(e.Message);
        }
    }
}