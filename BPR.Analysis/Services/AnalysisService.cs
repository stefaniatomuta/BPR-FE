using BPR.Mediator.Interfaces;
using BPR.Model.Architectures;
using BPR.Model.Enums;
using BPR.Model.Results;
using BPR.Analysis.Services.Analyses;

namespace BPR.Analysis.Services;

public class AnalysisService : IAnalysisService
{
    private readonly DependencyAnalysis _dependencyAnalysis;
    private readonly NamespaceAnalysis _namespaceAnalysis;

    public AnalysisService(DependencyAnalysis dependencyAnalysis, NamespaceAnalysis namespaceAnalysis)
    {
        _dependencyAnalysis = dependencyAnalysis;
        _namespaceAnalysis = namespaceAnalysis;
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
            violations.AddRange(await _namespaceAnalysis.AnalyseAsync(folderPath));
        }

        return violations;
    }
}