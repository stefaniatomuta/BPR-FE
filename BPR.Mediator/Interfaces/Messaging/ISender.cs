using BPR.Model.Rules;

namespace BPR.Mediator.Interfaces.Messaging;

public interface ISender
{
    Task<bool> SendAsync(string folderPath, List<Rule> rules, Guid correlationId);
}
