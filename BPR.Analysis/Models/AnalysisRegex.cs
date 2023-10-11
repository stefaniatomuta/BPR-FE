namespace BPR.Analysis.Models; 

public static class AnalysisRegex {
    public static string usingRegex { get; } = @"^using\s+[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z_][A-Za-z0-9_]*)*;$";
    public static string namespaceRegex { get;  }= @"^\s*namespace\s+[A-Za-z_][A-Za-z0-9_.]*\s*";
}