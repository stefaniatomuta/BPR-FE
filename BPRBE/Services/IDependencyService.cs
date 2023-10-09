using BPRBE.Models.Persistence;
using MongoDB.Bson;

namespace BPRBE.Services;

public interface IDependencyService
{
    /**
     * Mediator between the persistence and the UI
     * Used in retrieval of architectural models
     */
    Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
    Task<Result> AddModelAsync(ArchitecturalModel model);
    Task<Result> DeleteArchitectureModelAsync(ObjectId id);
}