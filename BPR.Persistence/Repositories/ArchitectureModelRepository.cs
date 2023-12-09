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

public class ArchitectureModelRepository : IDependencyRepository
{
    private readonly IMongoCollection<ArchitectureModelsCollection> _architectureModelsRuleCollection;
    private readonly ILogger<ArchitectureModelRepository> _logger;
    private readonly IMapper _mapper;

    public ArchitectureModelRepository(IOptions<DatabaseConfig> databaseSettings, ILogger<ArchitectureModelRepository> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _architectureModelsRuleCollection =
            mongoDatabase.GetCollection<ArchitectureModelsCollection>(databaseSettings.Value.ArchitectureModelsCollectionName);
        
    }

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        var collections = await (await _architectureModelsRuleCollection.FindAsync(_ => true)).ToListAsync();
        return _mapper.Map<IList<ArchitecturalModel>>(collections);
    }

    public async Task<Result> AddModelAsync(ArchitecturalModel model)
    {
        try
        {
            var result = await GetArchitecturalModelCollectionByName(model.Name);
            if (result != null) return Result.Fail<ArchitectureModelsCollection>("Model with the same name already exists", _logger);
            var modelCollection = _mapper.Map<ArchitectureModelsCollection>(model);
            await _architectureModelsRuleCollection.InsertOneAsync(modelCollection);
            _logger.LogInformation("Architecture model collection added" + modelCollection);
            return Result.Ok(modelCollection);
        }
        catch (Exception e)
        {
            return Result.Fail<ArchitectureModelsCollection>(e.Message, _logger);
        }
    }
    
    public async Task<Result> EditModelAsync(ArchitecturalModel model)
    {
        try
        {
            var result = await GetArchitecturalModelCollectionByName(model.Name);
            if (result != null && result.Id != model.Id)
            {
                return Result.Fail<ArchitectureModelsCollection>("Model with the same name already exists", _logger);
            }

            var filter = Builders<ArchitectureModelsCollection>.Filter.Eq(old => old.Id, model.Id);
            var update = Builders<ArchitectureModelsCollection>.Update
                .Set(old => old.Name, model.Name)
                .Set(old => old.Components, model.Components);
            
            await _architectureModelsRuleCollection.UpdateOneAsync(filter, update);
            _logger.LogInformation("Architectural model successfully edited");

            return Result.Ok(model);
        }
        catch (Exception e)
        {
            return Result.Fail<ArchitectureModelsCollection>(e.Message, _logger);
        }
    }

    public async Task<ArchitecturalModel?> DeleteModelAsync(Guid id)
    {
        var filter = Builders<ArchitectureModelsCollection>.Filter.Eq(model => model.Id, id);
        var result = await _architectureModelsRuleCollection.FindOneAndDeleteAsync(filter);
        
        return _mapper.Map<ArchitecturalModel?>(result);;
    }

    private async Task<ArchitectureModelsCollection?> GetArchitecturalModelCollectionByName(string name)
    {
        return await (await _architectureModelsRuleCollection.FindAsync(x => x.Name.Equals(name))).FirstOrDefaultAsync();
    }
}