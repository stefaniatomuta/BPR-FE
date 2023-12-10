using BPR.Model.Enums;

namespace BPR.Model.Rules;

public class Rule
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public RuleType RuleType { get; set; }
}