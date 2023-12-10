using BPR.Mediator.Utils;
using BPR.Model.Architectures;

namespace BPR.Mediator.Interfaces;

public interface IArchitectureModelService
{
    /**
     * Mediator between the persistence and the UI
     * Used in retrieval of architecture models
     */
    Task<IList<ArchitectureModel>> GetArchitectureModelsAsync();
    Task<Result> AddOrEditModelAsync(ArchitectureModel model);
    Task<Result> DeleteArchitectureModelAsync(Guid id);
}