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
            violations.AddRange(GetViolationsForComponentDependency(component.Name,folderPath, component.NamespaceComponents));
        }
        return violations;
    }

    private List<ViolationModel> GetViolationsForComponentDependency(string componentName, string folderPath, List<Namespace> namespaces) {
        List<ViolationModel> notMatched = new ();
        List<string> usings = new();

        foreach (var n in namespaces) {
            usings.AddRange(_codeExtractionService.GetUsingDirectives($"{folderPath}/{n.Name}"));
        }
        foreach (var directive in usings) {
            if (!namespaces.Select(n => n.Name).Any(n => n.Contains(directive))) {
                var violation = new ViolationModel{
                    Type = ViolationType.ForbiddenDependencyDirection,
                    Name = componentName,
                    Severity = ViolationSeverity.Major,
                    Code = directive
                };
                notMatched.Add(violation);
            }
        }
        return notMatched;
    }

    private List<string> GetUsingsForComponentNamespace(string folderPath, string componentNamespace) {
        
        return null;
    }

    private string GetFullPath(string folderPath, string componentNamespace) {
        return $"{folderPath}/{componentNamespace}";
    }
}