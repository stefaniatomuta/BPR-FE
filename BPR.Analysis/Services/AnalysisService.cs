using BPR.Mediator.Interfaces;
using BPR.Model.Architectures;
using BPR.Model.Enums;
using BPR.Model.Results;
using BPR.Analysis.Services.Analyses;

namespace BPR.Analysis.Services;

public class AnalysisService : IAnalysisService
{
    private DependencyAnalysis _dependencyAnalysis;

    public AnalysisService(DependencyAnalysis dependencyAnalysis)
    {
        _dependencyAnalysis = dependencyAnalysis;
    }

    public async Task<List<Violation>> GetAnalysisAsync(string folderPath, ArchitectureModel model, List<RuleType> ruleTypes)
    {
        var violations = new List<Violation>();

        if (ruleTypes.Contains(RuleType.ForbiddenDependency))
        {
            violations.AddRange(await _dependencyAnalysis.AnalyseAsync(folderPath, model));
        }

        if (ruleTypes.Contains(RuleType.MismatchedNamespace))
        {
            violations.AddRange(await NamespaceAnalysis.AnalyseAsync(folderPath));
        }

        return violations;
    }
}