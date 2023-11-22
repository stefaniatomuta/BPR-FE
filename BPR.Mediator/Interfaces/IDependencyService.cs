using BPR.Mediator.Utils;
using BPR.Model.Architectures;

namespace BPR.Mediator.Interfaces;

public interface IDependencyService
{
    /**
     * Mediator between the persistence and the UI
     * Used in retrieval of architectural models
     */
    Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
    Task<Result> AddOrEditModelAsync(ArchitecturalModel model);
    Task<Result> DeleteArchitectureModelAsync(Guid id);
}