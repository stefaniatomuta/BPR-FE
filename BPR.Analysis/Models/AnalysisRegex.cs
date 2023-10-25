namespace BPR.Analysis.Models;

public static class AnalysisRegex
{
    public const string UsingRegex = @"^using\s+[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z_][A-Za-z0-9_]*)*;$";
    public const string NamespaceRegex = @"^\s*namespace\s+[A-Za-z_][A-Za-z0-9_.]*\s*";
}