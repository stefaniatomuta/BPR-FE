using BPR.Mediator.Utils;
using BPR.Model.Results;

namespace BPR.Mediator.Interfaces;

public interface IResultRepository
{
    Task<Result<IList<AnalysisResult>>> GetAllResultsAsync();
    Task<Result<AnalysisResult>> GetResultAsync(Guid id);
    Task<Result<AnalysisResult>> AddResultAsync(AnalysisResult analysisResult);
    Task<Result<AnalysisResult>> UpdateResultAsync(AnalysisResult analysisResult);
    Task<Result> DeleteResultAsync(Guid id);
}