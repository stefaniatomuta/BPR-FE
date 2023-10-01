using BPR.Models.Analysis;
using BPR.Models.Blazor;
using BPR.Models.Enums;

namespace BPRBE.Services;

public class AnalysisService : IAnalysisService {

    private ICodeExtractionService _codeExtractionService;

    public AnalysisService(ICodeExtractionService codeExtractionService) {
        _codeExtractionService = codeExtractionService;
    }


    public List<ViolationModel> GetDependencyAnalysis(string folderPath, ArchitecturalModel model) {
        List<ViolationModel> violations = new();
        foreach (var component in model.Components) {
            violations.AddRange(GetViolationsForComponentDependency(component.Name, component.NamespaceComponents));
        }
        return violations;
    }

    private List<ViolationModel> GetViolationsForComponentDependency(string componentPath, List<Namespace> namespaces) {
        List<ViolationModel> notMatched = new ();
        var usings = _codeExtractionService.GetUsingDirectives(componentPath);
        foreach (var directive in usings) {
            if (!namespaces.Select(n => n.Name).Any(n => n.Contains(directive))) {
                var violation = new ViolationModel{
                    Type = ViolationType.ForbiddenDependencyDirection,
                    Name = componentPath,
                    Severity = ViolationSeverity.Major,
                    Code = directive
                };
                notMatched.Add(violation);
            }
        }
        return notMatched;
    }
}