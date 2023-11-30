using BPR.Model.Requests;

namespace BPR.Mediator.Interfaces;

public interface ISender
{
    Task SendAsync(MLAnalysisRequestModel request);
}
