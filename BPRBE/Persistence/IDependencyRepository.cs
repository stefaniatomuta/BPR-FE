using BPRBE.Models.Persistence;

namespace BPRBE.Persistence;

public interface IDependencyRepository
{
    Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
    Task<Result> AddModelAsync(ArchitecturalModel model);
    Task<ArchitecturalModel?> GetArchitecturalModelByName(ArchitecturalModel model);
    Task<ArchitecturalModel?> DeleteModelAsync(int modelId);
}