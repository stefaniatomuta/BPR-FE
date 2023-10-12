namespace BPRBE.Services.Models;

public class Rule
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}