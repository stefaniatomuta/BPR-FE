using BPR.Mediator.Utils;
using BPR.Model.Architectures;
using BPR.Model.Results;

namespace BPR.Mediator.Interfaces;

public interface IResultService
{
    Task<IList<AnalysisResult>> GetAllResultsAsync();
    Task<AnalysisResult?> GetResultAsync(Guid id);
    Task<Result> UpdateAndFinishResultAsync(Guid id, AnalysisResult result);
    Task<Result> CreateResultAsync(string folderPath, ArchitecturalModel model, List<Rule> rules);
    Task<Result> DeleteResultAsync(Guid id);
}