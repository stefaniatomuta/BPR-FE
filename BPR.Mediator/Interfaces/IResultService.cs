using BPR.Mediator.Utils;
using BPR.Model.Architectures;
using BPR.Model.Results;

namespace BPR.Mediator.Interfaces;

public interface IResultService
{
    Task<IList<AnalysisResult>> GetAllResultsAsync();
    Task<AnalysisResult?> GetResultAsync(Guid id);
    Task<Result> UpdateAndFinishResultAsync(Guid id, ExtendedAnalysisResults result);
    Task<Result<AnalysisResult>> CreateResultAsync(string folderPath, ArchitectureModel model, List<Rule> rules, string analysisTitle);
    Task<Result> DeleteResultAsync(Guid id);
}