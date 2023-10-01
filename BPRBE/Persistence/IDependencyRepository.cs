using BPRBE.Models.Persistence;

namespace BPRBE.Persistence;

public interface IDependencyRepository
{
    public Task<IList<MongoArchitecturalModel>> GetArchitecturalModelsAsync();
    public Task<Result> AddModelAsync(MongoArchitecturalModel model);
    public Task<MongoArchitecturalModel?> GetArchitecturalModelByName(MongoArchitecturalModel model);
}