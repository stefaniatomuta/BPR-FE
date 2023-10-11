namespace BPR.Analysis.Models; 

public class AnalysisRegex {
    public string usingRegex { get; } = @"^using\s+[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z_][A-Za-z0-9_]*)*;$";
    public string namespaceRegex { get;  }= @"^\s*namespace\s+[A-Za-z_][A-Za-z0-9_.]*\s*";
}