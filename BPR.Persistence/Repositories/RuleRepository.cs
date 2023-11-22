using AutoMapper;
using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Model.Architectures;
using BPR.Persistence.Collections;
using BPR.Persistence.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BPR.Persistence.Repositories;

public class RuleRepository : IRuleRepository
{
    private readonly IMongoCollection<RuleCollection> _rulesCollection;
    private readonly ILogger<RuleRepository> _logger;
    private readonly IMapper _mapper;

    public RuleRepository(IOptions<DatabaseConfig> databaseSettings, ILogger<RuleRepository> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;

        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _rulesCollection = mongoDatabase.GetCollection<RuleCollection>(databaseSettings.Value.RulesCollectionName);
    }
    
    public async Task<IList<Rule>> GetRulesAsync()
    {
        var results = await (await _rulesCollection.FindAsync(_ => true)).ToListAsync();
        var models = _mapper.Map<List<Rule>>(results);
        return models;
    }

    private async Task<RuleCollection?> GetRuleByNameAsync(string name)
    {
        return await (await _rulesCollection.FindAsync(x => x.Name.Equals(name))).FirstOrDefaultAsync();
    }
    
    public async Task<Result> AddRuleAsync(Rule rule)
    {
        try
        {
            var result = await GetRuleByNameAsync(rule.Name);
            if (result != null)
            {
                var message = $"RuleCollection with the same name {result.Name} already exists";
                return Result.Fail<RuleCollection>(message, _logger);
            }
            
            var ruleCollection = _mapper.Map<RuleCollection>(rule);
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