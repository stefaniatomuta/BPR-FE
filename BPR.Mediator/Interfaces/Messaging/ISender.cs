namespace BPR.Mediator.Interfaces.Messaging;

public interface ISender
{
    Task SendAsync(string folderPath, List<string> rules, Guid correlationId);
}
