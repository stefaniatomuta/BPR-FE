using BPRBE.Models.Persistence;

namespace BPRBE.Services;

public interface IDependencyService
{
    /**
     * Mediator between the persistence and the UI
     * Used in retrieval of architectural models
     */
    Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
    Task<Result> DeleteArchitectureModelAsync(int modelId);
}