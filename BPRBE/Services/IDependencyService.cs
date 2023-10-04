using BPRBE.Models.Persistence;

namespace BPRBE.Services;

public interface IDependencyService
{
    /**
     * Mediator between the persistence and the UI
     * Used in retrieval of architectural models
     */
    public Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
}