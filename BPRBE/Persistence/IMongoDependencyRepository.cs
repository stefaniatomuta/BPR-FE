using BPRBE.Models.Persistence;

namespace BPRBE.Persistence;

public interface IMongoDependencyRepository
{
    public Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
    public Task AddModelAsync(ArchitecturalModel model);
    public Task<ArchitecturalModel?> GetArchitecturalModelByName(ArchitecturalModel model);
}