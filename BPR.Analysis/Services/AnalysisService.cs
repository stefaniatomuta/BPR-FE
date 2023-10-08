using BPR.Analysis.Enums;
using BPR.Analysis.Models;

namespace BPR.Analysis.Services;

public class AnalysisService : IAnalysisService {

    private readonly ICodeExtractionService _codeExtractionService;

    public AnalysisService(ICodeExtractionService codeExtractionService) {
        _codeExtractionService = codeExtractionService;
    }


    public List<Violation> GetDependencyAnalysis(string folderPath, AnalysisArchitecturalModel model) {
        List<Violation> violations = new();
        var projectNames = _codeExtractionService.GetProjectNames(folderPath);

        foreach (var component in model.Components) {
            var usings = GetUsingsPerComponent(folderPath, component.NamespaceComponents);
            var usingsWithProjectNames = usings.Where(u => projectNames.Any(proj =>u.Using.Contains(proj))).ToList();
            foreach (var directive in usingsWithProjectNames) {
                var processedDirective = directive.Using.Replace(".", "/");
                if (!component.NamespaceComponents.Any(comp => processedDirective.Contains(comp.Name))) {
                    violations.Add(new Violation() {
                        Type = ViolationType.ForbiddenDependency,
                        Description = component.Name,
                        Severity = ViolationSeverity.Major,
                        Code = directive.Using,
                        File = directive.File
                    });
                }

                if (!component.Dependencies.Any(dep => processedDirective.Contains(dep.Name))) {
                    violations.Add(new Violation() {
                        Type = ViolationType.ForbiddenDependency,
                        Description = component.Name,
                        Severity = ViolationSeverity.Major,
                        Code = directive.Using,
                        File = directive.File
                    });
                }
            }
        }
        return violations;
    }

    private List<UsingDirective> GetUsingsPerComponent(string folderPath, List<AnalysisNamespace> namespaces) {
        List<UsingDirective> usings = new();

        foreach (var n in namespaces) {
            usings.AddRange(_codeExtractionService.GetUsingDirectives($"{folderPath}/{n.Name}"));
        }
        return usings.Distinct().ToList();
    }

    //TODO: refactor condition to ignore special chars like brackets
    public List<Violation> GetNamespaceAnalysis(string folderPath) {
        List<Violation> violations = new();
        var namespaces = _codeExtractionService.GetNamespaceDirectives(folderPath);
        foreach (var directive in namespaces) {
            var supposedNamespace = directive.FilePath.Split(folderPath)[1].Split(directive.File)[0].Replace("\\", ".");
            if (!supposedNamespace.Contains(directive.Namespace.Split("namespace")[1].Trim())) {
                violations.Add(new Violation() {
                    File = directive.File,
                    Severity = ViolationSeverity.Minor,
                    Code = directive.Namespace,
                    Description = supposedNamespace,
                    Type = ViolationType.MismatchedNamespace
                });
            }
        }
        return violations;
    }
}