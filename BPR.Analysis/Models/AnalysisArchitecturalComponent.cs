namespace BPR.Analysis.Models; 

public class AnalysisArchitecturalComponent {
    public int Id { get; set; }

    public string Style { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<AnalysisArchitecturalComponent> Dependencies { get; set; } = new();

    public List<AnalysisNamespace> NamespaceComponents { get; set; } = new();
}