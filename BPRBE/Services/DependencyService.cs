using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using BPRBE.Validators;

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
}