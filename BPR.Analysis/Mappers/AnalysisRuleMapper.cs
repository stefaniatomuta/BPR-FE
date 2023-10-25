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
            _ => default
        };
    }
}