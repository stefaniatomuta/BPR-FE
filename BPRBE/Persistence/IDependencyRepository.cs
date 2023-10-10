using BPRBE.Models.Persistence;

namespace BPRBE.Persistence;

public interface IDependencyRepository
{
    public Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
    public Task<Result> AddModelAsync(ArchitecturalModel model);
    public Task<ArchitecturalModel?> GetArchitecturalModelByName(ArchitecturalModel model);
}