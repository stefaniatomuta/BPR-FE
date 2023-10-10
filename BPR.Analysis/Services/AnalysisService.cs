using System.Globalization;
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
            var usingsWithProjectNames = usings.Where(u => projectNames.Any(proj => u.Using.Contains(proj))).ToList();
            
            foreach (var directive in usingsWithProjectNames) {
                var processedDirective = directive.Using.Replace(".", "/");
                
                if (!component.NamespaceComponents.Any(comp => processedDirective.Contains(comp.Name))
                    || !component.Dependencies.Any(dep => processedDirective.Contains(dep.Name))) {
                    
                    violations.Add(new Violation() {
                        Type = ViolationType.ForbiddenDependency,
                        Description = $"Dependency: {directive.Using} cannot be in {directive.File}. Component {component.Name} does not have this dependency",
                        Severity = ViolationSeverity.Major,
                        Code = directive.Using,
                        File = directive.File,
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

    public List<Violation> GetNamespaceAnalysis(string folderPath) {
        List<Violation> violations = new();
        var namespaces = _codeExtractionService.GetNamespaceDirectives(folderPath);
        
        foreach (var directive in namespaces) {
            var supposedNamespace = directive.FilePath.Split(folderPath)[1].Split(directive.File)[0].Replace("\\", ".");
            var currentNamespace = directive.Namespace.Split("namespace")[1].Trim();
            var comparison = string.Compare(currentNamespace,0, supposedNamespace,0,
                supposedNamespace.Length,CultureInfo.InvariantCulture, CompareOptions.IgnoreSymbols);
            
            if (comparison != 0) {
                violations.Add(new Violation() {
                        File = directive.File,
                        Severity = ViolationSeverity.Minor,
                        Code = directive.Namespace,
                        Description = $"Namespace \"{directive}\" in {directive.File} does not match.",
                        Type = ViolationType.MismatchedNamespace
                    });
            }
        }
        return violations;
    }
}