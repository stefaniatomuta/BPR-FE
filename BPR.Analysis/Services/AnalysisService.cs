using BPR.Analysis.Enums;
using BPR.Analysis.Models;

namespace BPR.Analysis.Services;

public class AnalysisService : IAnalysisService {

    private readonly ICodeExtractionService _codeExtractionService;

    public AnalysisService(ICodeExtractionService codeExtractionService) {
        _codeExtractionService = codeExtractionService;
    }

//TODO: refactor to add filename for violation
    public List<Violation> GetDependencyAnalysis(string folderPath, AnalysisArchitecturalModel model) {
        List<Violation> violations = new();
        var projectNames = _codeExtractionService.GetProjectNames(folderPath);

        foreach (var component in model.Components) {
            var usings = GetUsingsPerComponent(folderPath, component.NamespaceComponents);
            var usingsWithProjectNames = usings.Where(u => projectNames.Any(proj =>u.Contains(proj))).ToList();
            foreach (var directive in usingsWithProjectNames) {
                var processedDirective = directive.Replace(".", "/");
                if (!component.NamespaceComponents.Any(comp => processedDirective.Contains(comp.Name)) ||
                    !component.Dependencies.Any(dep => processedDirective.Contains(dep.Name))) {
                    violations.Add(new Violation() {
                        Type = ViolationType.ForbiddenDependency,
                        Description = component.Name,
                        Severity = ViolationSeverity.Major,
                        Code = directive,
                    });
                }
            }
        }
        return violations;
    }

    private List<string> GetUsingsPerComponent(string folderPath, List<AnalysisNamespace> namespaces) {
        List<string> usings = new();

        foreach (var n in namespaces) {
            usings.AddRange(_codeExtractionService.GetUsingDirectives($"{folderPath}/{n.Name}"));
        }
        return usings.Distinct().ToList();
    }

}