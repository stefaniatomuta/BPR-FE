using BPR.Persistence.Models;
using BPR.Persistence.Utils;

namespace BPR.Persistence.Repositories;

public interface IResultRepository
{
    Task<Result<List<ResultCollection>>> GetAllResultsAsync();
    Task<Result<ResultCollection>> GetResultAsync(Guid id);
    Task<Result<ResultCollection>> AddResultAsync(ResultCollection modelCollection);
    Task<Result<ResultCollection>> UpdateResultAsync(ResultCollection modelCollection);
    Task<Result> DeleteResultAsync(Guid id);
}