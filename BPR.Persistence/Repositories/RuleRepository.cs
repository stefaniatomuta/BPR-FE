using BPR.Persistence.Config;
using BPR.Persistence.Models;
using BPR.Persistence.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BPR.Persistence.Repositories;

public class RuleRepository : IRuleRepository
{
    private readonly IMongoCollection<RuleCollection> _rulesCollection;
    private readonly ILogger<RuleRepository> _logger;

    public RuleRepository(IOptions<DatabaseConfig> databaseSettings, ILogger<RuleRepository> logger)
    {
        _logger = logger;
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _rulesCollection = mongoDatabase.GetCollection<RuleCollection>(databaseSettings.Value.RulesCollectionName);
    }
    
    public async Task<IList<RuleCollection>> GetRulesAsync()
    {
        return await (await _rulesCollection.FindAsync(_ => true)).ToListAsync();
    }

    private async Task<RuleCollection?> GetRuleByNameAsync(string name)
    {
        return await (await _rulesCollection.FindAsync(x => x.Name.Equals(name))).FirstOrDefaultAsync();
    }
    
    public async Task<Result> AddRuleAsync(RuleCollection ruleCollection)
    {
        try
        {
            var result = await GetRuleByNameAsync(ruleCollection.Name);
            if (result != null)
            {
                var message = $"RuleCollection with the same name {result.Name} already exists";
                return Result.Fail<RuleCollection>(message, _logger);
            }
            
            await _rulesCollection.InsertOneAsync(ruleCollection);
            _logger.LogInformation($"Architectural model added" + ruleCollection);
            
            return Result.Ok(ruleCollection);
        }
        catch (Exception e)
        {
            return Result.Fail<RuleCollection>(e.Message, _logger);
        }
    }
}