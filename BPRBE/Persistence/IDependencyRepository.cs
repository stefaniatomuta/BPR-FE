using BPRBE.Models.Persistence;
using MongoDB.Bson;

namespace BPRBE.Persistence;

public interface IDependencyRepository
{
    Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync();
    Task<Result> AddModelAsync(ArchitecturalModel model);
    Task<ArchitecturalModel?> DeleteModelAsync(ObjectId id);
}