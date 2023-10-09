using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using BPRBE.Validators;
using MongoDB.Bson;

namespace BPRBE.Services;

public class DependencyService : IDependencyService
{
    private readonly IDependencyRepository _dependencyRepository;
    private readonly IValidatorService _validatorService;

    public DependencyService(IDependencyRepository dependencyRepository, IValidatorService validatorService)
    {
        _dependencyRepository = dependencyRepository;
        _validatorService = validatorService;
    }

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        return await _dependencyRepository.GetArchitecturalModelsAsync();
    }

    public async Task<Result> AddModelAsync(ArchitecturalModel model)
    {
        var result = await _validatorService.ValidateArchitecturalModelAsync(model);
        if (result.Success)
        {
            return await _dependencyRepository.AddModelAsync(model);
        }

        return result;
    }

    public async Task<Result> DeleteArchitectureModelAsync(ObjectId modelId)
    {
        /* TODO - Change to the Guid type. 'int' as id is used throughout the UI as well. Will need to be changed.
           A guid is IIRC not null by default, but "Empty" (all 0s), so validate that an actual guid has been passed */
        var deletedModel = await _dependencyRepository.DeleteModelAsync(modelId);

        return deletedModel is not null
            ? new Result(true)
            : new Result(false, $"No architecture model found with id: '{modelId}'");
    }
}