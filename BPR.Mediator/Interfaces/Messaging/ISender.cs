using BPR.Model.Architectures;

namespace BPR.Mediator.Interfaces.Messaging;

public interface ISender
{
    bool Send(string folderPath, List<Rule> rules, Guid correlationId);
}
