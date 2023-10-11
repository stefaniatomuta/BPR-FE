namespace BPR.Analysis.Models; 

public class NamespaceDirective {
    public string Namespace { get; set; } = null!;
    public string File { get; set; } = null!;
    public string FilePath { get; set; } = null!;
}