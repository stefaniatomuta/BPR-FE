using BPR.Mediator.Models;
using BPR.Persistence.Utils;

namespace BPR.Mediator.Services;

public interface IResultService
{
    Task<IList<ResultModel>> GetAllResultsAsync();
    Task<ResultModel> GetResultAsync(Guid id);
    Task<Result> CreateResultAsync(string folderPath, ArchitecturalModel model, List<Rule> rules);
    Task<Result> DeleteResultAsync(Guid id);
}