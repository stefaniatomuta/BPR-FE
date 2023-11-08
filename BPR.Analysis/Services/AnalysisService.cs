using BPR.Analysis.Enums;
using BPR.Analysis.Models;
using System.Globalization;

namespace BPR.Analysis.Services;

public class AnalysisService : IAnalysisService
{
    private readonly ICodeExtractionService _codeExtractionService;

    public AnalysisService(ICodeExtractionService codeExtractionService)
    {
        _codeExtractionService = codeExtractionService;
    }

    public List<Violation> GetAnalysis(string folderPath, AnalysisArchitecturalModel model, List<AnalysisRule> rules)
    {
        List<Violation> violations = new();

        if (rules.Contains(AnalysisRule.Dependency))
        {
            violations.AddRange(GetDependencyAnalysis(folderPath, model));
        }
        
        if (rules.Contains(AnalysisRule.Namespace))
        {
            violations.AddRange(GetNamespaceAnalysis(folderPath));
        }

        return violations;
    }

    private List<Violation> GetDependencyAnalysis(string folderPath, AnalysisArchitecturalModel model)
    {
        var projectNames = _codeExtractionService.GetProjectNames(folderPath);
        List<Violation> violations = GetDependencyAnalysisOnNamespaceComponents(folderPath, projectNames, model.Components);

        foreach (var component in model.Components)
        {
            violations.AddRange(GetDependencyAnalysisOnNamespaceComponents(folderPath, projectNames, component.Dependencies));
        }

        return violations;
    }

    private List<Violation> GetDependencyAnalysisOnNamespaceComponents(string folderPath, List<string> projectNames, List<AnalysisArchitecturalComponent> components)
    {
        List<Violation> violations = new();
        foreach (var component in components)
        {
            var usings = GetUsingsPerComponent(folderPath, component.NamespaceComponents);
            var usingsWithProjectNames = usings.Where(u => projectNames.Any(proj => u.Using.Contains(proj) && !component.NamespaceComponents.Any(ns => proj.Contains(ns.Name)))).ToList();
            violations.AddRange(GetDependencyAnalysisOnComponent(usingsWithProjectNames, component));
        }
        return violations;
    }

    public List<Violation> GetDependencyAnalysisOnComponent(List<UsingDirective> usingDirectives, AnalysisArchitecturalComponent component)
    {
        List<Violation> violations = new();
        foreach (var directive in usingDirectives)
        {
            if (component.Dependencies
                .SelectMany(dep => dep.NamespaceComponents)
                .Any(ns => !directive.Using.Contains(ns.Name)))
            {
                violations.Add(new Violation()
                {
                    Type = ViolationType.ForbiddenDependency,
                    Description = $"'{directive.Using}' cannot be in '{directive.FilePath}'. Component '{component.Name}' does not have this dependency",
                    Severity = ViolationSeverity.Major,
                    Code = directive.Using,
                    File = directive.File,
                });
            }
        }
        return violations;
    }

    private List<UsingDirective> GetUsingsPerComponent(string folderPath, List<AnalysisNamespace> namespaces)
    {
        List<UsingDirective> usings = new();

        foreach (var n in namespaces)
        {
            usings.AddRange(_codeExtractionService.GetUsingDirectives($"{folderPath}/{n.Name}"));
        }
        return usings.Distinct().ToList();
    }

    public List<Violation> GetNamespaceAnalysis(string folderPath)
    {
        var namespaces = _codeExtractionService.GetNamespaceDirectives(folderPath);
        return GetNamespaceAnalysis(namespaces, folderPath);
    }

    public List<Violation> GetNamespaceAnalysis(List<NamespaceDirective> namespaces, string folderPath)
    {
        List<Violation> violations = new();

        foreach (var directive in namespaces)
        {
            var supposedNamespace = directive.FilePath.Split(folderPath)[1].Split(directive.File)[0].Replace("\\", ".");
            var currentNamespace = directive.Namespace.Split("namespace")[1].Trim();
            var comparison = string.Compare(currentNamespace, 0, supposedNamespace, 0,
                supposedNamespace.Length, CultureInfo.InvariantCulture, CompareOptions.IgnoreSymbols);

            if (comparison != 0)
            {
                violations.Add(new Violation()
                {
                    File = directive.File,
                    Severity = ViolationSeverity.Minor,
                    Code = directive.Namespace,
                    Description = $"Namespace '{directive.Namespace}' in '{directive.File}' does not match",
                    Type = ViolationType.MismatchedNamespace
                });
            }
        }
        return violations;
    }
}