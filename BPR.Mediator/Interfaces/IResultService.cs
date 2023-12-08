using BPR.Mediator.Utils;
using BPR.Model.Architectures;
using BPR.Model.Results;
using BPR.Model.Results.External;

namespace BPR.Mediator.Interfaces;

public interface IResultService
{
    Task<IList<AnalysisResult>> GetAllResultsAsync();
    Task<AnalysisResult?> GetResultAsync(Guid id);
    Task<Result> UpdateAndFinishResultAsync(Guid id, ExtendedAnalysisResults result);
    Task<Result<AnalysisResult>> CreateResultAsync(string folderPath, ArchitecturalModel model, List<Rule> rules, string analysisTitle);
    Task<Result> DeleteResultAsync(Guid id);
}