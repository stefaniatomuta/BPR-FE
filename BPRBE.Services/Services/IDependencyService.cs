using BPR.Persistence.Utils;
using BPRBE.Services.Models;

namespace BPRBE.Services.Services;

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