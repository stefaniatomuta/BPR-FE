using BPR.Persistence.Models;
using BPR.Persistence.Utils;
using MongoDB.Bson;

namespace BPR.Persistence.Repositories;

public interface IDependencyRepository
{
    Task<IList<ArchitecturalModelCollection>> GetArchitecturalModelsAsync();
    Task<Result> AddModelAsync(ArchitecturalModelCollection modelCollection);
    Task<ArchitecturalModelCollection?> DeleteModelAsync(ObjectId id);
}