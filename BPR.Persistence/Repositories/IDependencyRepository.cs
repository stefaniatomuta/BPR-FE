using BPR.Persistence.Models;
using BPR.Persistence.Utils;

namespace BPR.Persistence.Repositories;

public interface IDependencyRepository
{
    Task<IList<ArchitecturalModelCollection>> GetArchitecturalModelsAsync();
    Task<Result> AddModelAsync(ArchitecturalModelCollection modelCollection);
    Task<ArchitecturalModelCollection?> DeleteModelAsync(Guid id);
    Task<Result> EditModelAsync(ArchitecturalModelCollection model);
}