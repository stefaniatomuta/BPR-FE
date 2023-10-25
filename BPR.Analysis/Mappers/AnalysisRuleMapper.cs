using System.ComponentModel;
using BPR.Analysis.Enums;

namespace BPR.Analysis.Mappers;

public static class AnalysisRuleMapper
{
    public static AnalysisRule GetAnalysisRuleEnum(string name)
    {
        return name switch
        {
            "Dependency" => AnalysisRule.Dependency,
            "Namespace" => AnalysisRule.Namespace,
            _ => throw new InvalidEnumArgumentException("No valid rule with this name")
        };
    }
}