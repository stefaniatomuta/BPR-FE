namespace BPR.Analysis.Models; 

public class AnalysisArchitecturalModel {
    public string Name { get; set; } = string.Empty;
    
    public List<AnalysisArchitecturalComponent> Components { get; set; } = new();
}