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

public class DependencyRepository : IDependencyRepository
{
    private readonly IMongoCollection<ArchitecturalModelCollection> _dependenciesRuleCollection;
    private readonly ILogger<DependencyRepository> _logger;
    private readonly IMapper _mapper;

    public DependencyRepository(IOptions<DatabaseConfig> databaseSettings, ILogger<DependencyRepository> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _dependenciesRuleCollection =
            mongoDatabase.GetCollection<ArchitecturalModelCollection>(databaseSettings.Value.DependenciesRuleCollectionName);
        
    }

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        var collections = await (await _dependenciesRuleCollection.FindAsync(_ => true)).ToListAsync();
        return _mapper.Map<IList<ArchitecturalModel>>(collections);
    }

    public async Task<Result> AddModelAsync(ArchitecturalModel model)
    {
        try
        {
            var result = await GetArchitecturalModelCollectionByName(model.Name);
            if (result != null) return Result.Fail<ArchitecturalModelCollection>("Model with the same name already exists", _logger);
            var modelCollection = _mapper.Map<ArchitecturalModelCollection>(model);
            await _dependenciesRuleCollection.InsertOneAsync(modelCollection);
            _logger.LogInformation("Architectural modelCollection added" + modelCollection);
            return Result.Ok(modelCollection);
        }
        catch (Exception e)
        {
            return Result.Fail<ArchitecturalModelCollection>(e.Message, _logger);
        }
    }
    
    public async Task<Result> EditModelAsync(ArchitecturalModel model)
    {
        try
        {
            var result = await GetArchitecturalModelCollectionByName(model.Name);
            if (result != null && result.Id != model.Id)
            {
                return Result.Fail<ArchitecturalModelCollection>("Model with the same name already exists", _logger);
            }

            var filter = Builders<ArchitecturalModelCollection>.Filter.Eq(old => old.Id, model.Id);
            var update = Builders<ArchitecturalModelCollection>.Update
                .Set(old => old.Name, model.Name)
                .Set(old => old.Components, model.Components);
            
            await _dependenciesRuleCollection.UpdateOneAsync(filter, update);
            _logger.LogInformation("Architectural model successfully edited");

            return Result.Ok(model);
        }
        catch (Exception e)
        {
            return Result.Fail<ArchitecturalModelCollection>(e.Message, _logger);
        }
    }

    public async Task<ArchitecturalModel?> DeleteModelAsync(Guid id)
    {
        var filter = Builders<ArchitecturalModelCollection>.Filter.Eq(model => model.Id, id);
        var result = await _dependenciesRuleCollection.FindOneAndDeleteAsync(filter);
        
        return _mapper.Map<ArchitecturalModel?>(result);;
    }

    private async Task<ArchitecturalModelCollection?> GetArchitecturalModelCollectionByName(string name)
    {
        return await (await _dependenciesRuleCollection.FindAsync(x => x.Name.Equals(name))).FirstOrDefaultAsync();
    }
}