using BPRBE.Models.Persistence;
using BPRBE.Persistence;

namespace BPRBE.Services;

public class DependencyService : IDependencyService
{
    private readonly IDependencyRepository _dependencyRepository;

    public DependencyService(IDependencyRepository dependencyRepository)
    {
        _dependencyRepository = dependencyRepository;
    }

    public Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        return _dependencyRepository.GetArchitecturalModelsAsync();
    }

    public async Task<Result> DeleteArchitectureModelAsync(int modelId)
    {
        /* TODO - Change to the Guid type. 'int' as id is used throughout the UI as well. Will need to be changed.
           A guid is IIRC not null by default, but "Empty" (all 0s), so validate that an actual guid has been passed */
        var deletedModel = await _dependencyRepository.DeleteModelAsync(modelId);

        return deletedModel is not null
            ? new Result(true)
            : new Result(false, $"No architecture model found with id: '{modelId}'");
    }
}