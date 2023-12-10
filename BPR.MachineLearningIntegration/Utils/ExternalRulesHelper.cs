using BPR.MachineLearningIntegration.Enums;
using BPR.Model.Architectures;
using BPR.Model.Enums;

namespace BPR.MachineLearningIntegration.Utils;

public static class ExternalRulesHelper
{
    public static List<string> ToExternalAnalysisRules(List<Rule> rules)
    {
        var output = new List<string>();

        foreach (var rule in rules)
        {
            output.AddRange(rule.ViolationType switch
            {
                ViolationType.ConditionalStatements => Enum.GetValues<ConditionalStatements>().Select(e => e.ToString()),
                ViolationType.MismatchedNamespace => Enum.GetValues<MismatchedNamespace>().Select(e =>e.ToString()),
                ViolationType.SolutionMetrics => Enum.GetValues<SolutionMetrics>().Select(e => e.ToString()),
                ViolationType.ExternalCalls => Enum.GetValues<ExternalCalls>().Select(e => e.ToString()),
                ViolationType.CodeSimilarity => Enum.GetValues<CodeSimilarity>().Select(e => e.ToString()),
                _ => Array.Empty<string>()
            });
        }

        return output;
    }
}
