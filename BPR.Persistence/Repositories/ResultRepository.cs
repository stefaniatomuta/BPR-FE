using BPR.Persistence.Config;
using BPR.Persistence.Models;
using BPR.Persistence.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BPR.Persistence.Repositories;

public class ResultRepository : IResultRepository
{
    private readonly IMongoCollection<ResultCollection> _resultCollection;
    private readonly ILogger<RuleRepository> _logger;

    public ResultRepository(IOptions<DatabaseConfig> databaseSettings, ILogger<RuleRepository> logger)
    {
        _logger = logger;
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _resultCollection = mongoDatabase.GetCollection<ResultCollection>(databaseSettings.Value.ResultsCollectionName);
    }

    public async Task<Result<List<ResultCollection>>> GetAllResultsAsync()
    {
        var list = await (await _resultCollection.FindAsync(_ => true)).ToListAsync();
        return Result.Ok(list);
    }

    public async Task<Result<ResultCollection>> GetResultAsync(Guid id)
    {
        var result = await (await _resultCollection.FindAsync(x => x.Id.Equals(id))).FirstOrDefaultAsync();
        return Result.Ok(result);
    }

    public async Task<Result<ResultCollection>> AddResultAsync(ResultCollection modelCollection)
    {
        try
        {
            var result = await GetResultAsync(modelCollection.Id);
            if (result.Value != null)
            {
                modelCollection.Id = new Guid();
                await AddResultAsync(modelCollection);
            }

            await _resultCollection.InsertOneAsync(modelCollection);
            return Result.Ok(modelCollection);
        }
        catch (Exception e)
        {
            return Result.Fail<ResultCollection>(e.Message, _logger);
        }    
    }
    
    public async Task<Result<ResultCollection>> UpdateResultAsync(ResultCollection modelCollection)
    {
        try
        {
            var filter = Builders<ResultCollection>.Filter.Eq(old => old.Id, modelCollection.Id);
            var update = Builders<ResultCollection>.Update
                .Set(old => old.Score, modelCollection.Score)
                .Set(old => old.ResultEnd, modelCollection.ResultEnd)
                .Set(old => old.Violations, modelCollection.Violations)
                .Set(old => old.ResultStatus, modelCollection.ResultStatus);
            
            await _resultCollection.UpdateOneAsync(filter, update);
            return Result.Ok(modelCollection);
        }
        catch (Exception e)
        {
            return Result.Fail<ResultCollection>(e.Message, _logger);
        }    
    }

    public async Task<Result> DeleteResultAsync(Guid id)
    {
        var filter = Builders<ResultCollection>.Filter.Eq(model => model.Id, id);
        await _resultCollection.FindOneAndDeleteAsync(filter);
        return new Result(true);    
    }
}