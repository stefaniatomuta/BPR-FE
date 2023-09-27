namespace BPRBE.Models; 

public class Dependency
{
    public string Component { get; set; } = string.Empty;
    public string DependsOn { get; set; } = string.Empty;
}