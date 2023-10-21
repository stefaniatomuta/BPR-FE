namespace BPR.Analysis.Models;

public static class AnalysisRegex
{
    public static string UsingRegex = @"^using\s+[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z_][A-Za-z0-9_]*)*;$";
    public static string NamespaceRegex = @"^\s*namespace\s+[A-Za-z_][A-Za-z0-9_.]*\s*";
}