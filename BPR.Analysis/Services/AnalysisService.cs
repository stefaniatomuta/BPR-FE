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

    public async Task<List<Violation>> GetAnalysisAsync(string folderPath, ArchitectureModel model, List<ViolationType> violationTypes)
    {
        var violations = new List<Violation>();

        if (violationTypes.Contains(ViolationType.ForbiddenDependency))
        {
            violations.AddRange(await _dependencyAnalysis.AnalyseAsync(folderPath, model));
        }

        if (violationTypes.Contains(ViolationType.MismatchedNamespace))
        {
            violations.AddRange(await NamespaceAnalysis.AnalyseAsync(folderPath));
        }

        return violations;
    }
}