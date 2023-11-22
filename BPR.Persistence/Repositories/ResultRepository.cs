using AutoMapper;
using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Model.Results;
using BPR.Persistence.Collections;
using BPR.Persistence.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BPR.Persistence.Repositories;

public class ResultRepository : IResultRepository
{
    private readonly IMongoCollection<ResultCollection> _resultCollection;
    private readonly ILogger<RuleRepository> _logger;
    private readonly IMapper _mapper;

    public ResultRepository(IOptions<DatabaseConfig> databaseSettings, ILogger<RuleRepository> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _resultCollection = mongoDatabase.GetCollection<ResultCollection>(databaseSettings.Value.ResultsCollectionName);
    }

    public async Task<Result<IList<AnalysisResult>>> GetAllResultsAsync()
    {
        var list = await (await _resultCollection.FindAsync(_ => true)).ToListAsync();
        var models = _mapper.Map<IList<AnalysisResult>>(list);
        
        return Result.Ok(models);
    }

    public async Task<Result<AnalysisResult>> GetResultAsync(Guid id)
    {
        var result = await (await _resultCollection.FindAsync(x => x.Id.Equals(id))).FirstOrDefaultAsync();
        var model = _mapper.Map<AnalysisResult>(result);
        
        return Result.Ok(model);
    }

    public async Task<Result<AnalysisResult>> AddResultAsync(AnalysisResult model)
    {
        try
        {
            var result = await GetResultAsync(model.Id);
            if (result.Value != null)
            {
                model.Id = new Guid();
                await AddResultAsync(model);
            }

            var collection = _mapper.Map<ResultCollection>(model);
            await _resultCollection.InsertOneAsync(collection);
            model = _mapper.Map<AnalysisResult>(collection);
            return Result.Ok(model);
        }
        catch (Exception e)
        {
            return Result.Fail<AnalysisResult>(e.Message, _logger);
        }    
    }
    
    public async Task<Result<AnalysisResult>> UpdateResultAsync(AnalysisResult model)
    {
        try
        {
            var filter = Builders<ResultCollection>.Filter.Eq(old => old.Id, model.Id);
            var update = Builders<ResultCollection>.Update
                .Set(old => old.Score, model.Score)
                .Set(old => old.ResultEnd, model.ResultEnd)
                .Set(old => old.Violations, model.Violations)
                .Set(old => old.ResultStatus, model.ResultStatus);
            
            await _resultCollection.UpdateOneAsync(filter, update);
            return Result.Ok(model);
        }
        catch (Exception e)
        {
            return Result.Fail<AnalysisResult>(e.Message, _logger);
        }    
    }

    public async Task<Result> DeleteResultAsync(Guid id)
    {
        var filter = Builders<ResultCollection>.Filter.Eq(model => model.Id, id);
        await _resultCollection.FindOneAndDeleteAsync(filter);
        return new Result(true);    
    }
}