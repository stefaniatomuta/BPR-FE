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

    public async Task<List<Violation>> GetAnalysisAsync(string folderPath, AnalysisArchitecturalModel model, List<AnalysisRule> rules, bool isOpenArchitecture)
    {
        List<Violation> violations = new();

        if (rules.Contains(AnalysisRule.Dependency))
        {
            violations.AddRange(await GetDependencyAnalysisAsync(folderPath, model, isOpenArchitecture));
        }
        
        if (rules.Contains(AnalysisRule.Namespace))
        {
            violations.AddRange(await GetNamespaceAnalysisAsync(folderPath));
        }

        return violations;
    }

    private async Task<List<Violation>> GetDependencyAnalysisAsync(string folderPath, AnalysisArchitecturalModel model, bool isOpenArchitecture)
    {
        var projectNames = _codeExtractionService.GetProjectNames(folderPath);
        List<Violation> violations = await GetDependencyAnalysisOnNamespaceComponentsAsync(folderPath, projectNames, model.Components, isOpenArchitecture);

        foreach (var component in model.Components)
        {
            violations.AddRange(await GetDependencyAnalysisOnNamespaceComponentsAsync(folderPath, projectNames, component.Dependencies, isOpenArchitecture));
        }

        return violations;
    }

    private async Task<List<Violation>> GetDependencyAnalysisOnNamespaceComponentsAsync(string folderPath, List<string> projectNames, List<AnalysisArchitecturalComponent> components, bool isOpenArchitecture)
    {
        List<Violation> violations = new();

        foreach (var component in components)
        {
            var usings = await GetUsingsPerComponentAsync(folderPath, component.NamespaceComponents);
            var usingsWithProjectNames = usings.Where(u => projectNames.Any(proj => u.Using.Contains(proj))).ToList();
            violations.AddRange(GetDependencyAnalysisOnComponent(usingsWithProjectNames, component, isOpenArchitecture));
        }

        return violations;
    }

    internal static List<Violation> GetDependencyAnalysisOnComponent(IList<UsingDirective> usingDirectives, AnalysisArchitecturalComponent component, bool isOpenArchitecture)
    {
        List<Violation> violations = new();

        foreach (var directive in usingDirectives)
        {
            if ((!component.Dependencies
                .SelectMany(dep => dep.NamespaceComponents)
                .Any(ns => directive.Using.Contains(ns.Name)) // a using statement in X to Y but X does not have Y as dependency
                || component.NamespaceComponents.Any(nc => nc.Name == directive.ComponentName)
                && !component.Dependencies.Any()) // a using statement in X to Y but component has no dependencies, i.e X cannot have dependency to Y
                && !directive.Using.Contains(directive.ComponentName) // ignore away self-dependencies, i.e. a using statement in X to X
                && (!isOpenArchitecture || !IsUsingRecursiveDependency(directive, component, component.Dependencies))) // check for recursive dependencies if model is considered "Open"
            {
                violations.Add(new Violation()
                {
                    Type = ViolationType.ForbiddenDependency,
                    Description = $"'{directive.Using}' cannot be in '{directive.FilePath}'. Component '{directive.ComponentName}' in '{component.Name}' cannot have this dependency",
                    Severity = ViolationSeverity.Major,
                    Code = directive.Using,
                    File = directive.File,
                });
            }
        }

        return violations;
    }

    private static bool IsUsingRecursiveDependency(UsingDirective directive, AnalysisArchitecturalComponent component, List<AnalysisArchitecturalComponent> dependencies)
    {
        foreach (var nameSpace in dependencies.SelectMany(dep => dep.NamespaceComponents))
        {
            if (directive.Using.Contains(nameSpace.Name) && component.NamespaceComponents.Any(nc => nc.Name == directive.ComponentName))
            {
                return true;
            }
        }

        foreach (var innerDependencies in dependencies.Select(dep => dep.Dependencies))
        {
            return IsUsingRecursiveDependency(directive, component, innerDependencies);
        }

        return false;
    }

    private async Task<IList<UsingDirective>> GetUsingsPerComponentAsync(string folderPath, List<AnalysisNamespace> namespaces)
    {
        List<UsingDirective> usings = new();

        foreach (var n in namespaces) {
            var result = await _codeExtractionService.GetUsingDirectivesAsync($"{folderPath}/{n.Name}");
            result.ForEach(u => u.FilePath = u.FilePath.Split($"{folderPath}/")[1]);
            result.ForEach(u => u.ComponentName = n.Name);
            usings.AddRange(result);
        }

        return usings.Distinct().ToList();
    }

    private async Task<List<Violation>> GetNamespaceAnalysisAsync(string folderPath)
    {
        var namespaces = await _codeExtractionService.GetNamespaceDirectivesAsync(folderPath);
        return GetNamespaceAnalysis(namespaces, folderPath);
    }

    internal static List<Violation> GetNamespaceAnalysis(List<NamespaceDirective> namespaces, string folderPath)
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