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

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        return await _dependencyRepository.GetArchitecturalModelsAsync();
    }
}