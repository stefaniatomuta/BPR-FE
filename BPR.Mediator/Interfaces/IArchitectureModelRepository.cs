using BPR.Mediator.Utils;
using BPR.Model.Architectures;

namespace BPR.Mediator.Interfaces;

public interface IArchitectureModelRepository
{
    Task<IList<ArchitectureModel>> GetArchitectureModelsAsync();
    Task<Result> AddModelAsync(ArchitectureModel model);
    Task<ArchitectureModel?> DeleteModelAsync(Guid id);
    Task<Result> EditModelAsync(ArchitectureModel model);
}