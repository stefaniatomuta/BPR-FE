using BPR.Mediator.Utils;
using BPR.Model.Api;
using BPR.Model.Architectures;
using BPR.Model.Results;

namespace BPR.Mediator.Interfaces;

public interface IResultService
{
    Task<IList<AnalysisResult>> GetAllResultsAsync();
    Task<AnalysisResult?> GetResultAsync(Guid id);
    Task<Result> UpdateAndFinishResultAsync(Guid id, MLAnalysisResponseModel result);
    Task<Result<AnalysisResult>> CreateResultAsync(string folderPath, ArchitecturalModel model, List<Rule> rules);
    Task<Result> DeleteResultAsync(Guid id);
}