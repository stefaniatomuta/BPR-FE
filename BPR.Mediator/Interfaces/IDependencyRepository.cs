using BPR.Mediator.Utils;
using BPR.Model.Architectures;

namespace BPR.Mediator.Interfaces;

public interface IDependencyRepository
{
    Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
    Task<Result> AddModelAsync(ArchitecturalModel modelCollection);
    Task<ArchitecturalModel?> DeleteModelAsync(Guid id);
    Task<Result> EditModelAsync(ArchitecturalModel model);
}