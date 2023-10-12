using BPR.Mediator.Models;
using BPR.Persistence.Utils;

namespace BPR.Mediator.Services;

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