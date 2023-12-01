﻿using BPR.Analysis.Helpers;
using BPR.Analysis.Models;
using BPR.Mediator.Interfaces;
using BPR.Model.Architectures;
using BPR.Model.Results;
using System.Text;
using System.Text.RegularExpressions;

namespace BPR.Analysis.Services.Analyses;

public class DependencyAnalysis
{
    private readonly ICodeExtractionService _codeExtractionService;

    public DependencyAnalysis(ICodeExtractionService codeExtractionService)
    {
        _codeExtractionService = codeExtractionService;
    }

    internal async Task<List<Violation>> AnalyseAsync(string folderPath, ArchitecturalModel model)
    {
        var projectNames = _codeExtractionService.GetProjectNames(folderPath);
        var violations = new List<Violation>();

        foreach (var component in model.Components)
        {
            violations.AddRange(await GetDependencyAnalysisOnComponentAsync(folderPath, model, projectNames, component));
        }

        return violations;
    }

    private async Task<List<Violation>> GetDependencyAnalysisOnComponentAsync(
        string folderPath,
        ArchitecturalModel model,
        IList<string> projectNames,
        ArchitecturalComponent component)
    {
        var componentUsings = await GetUsingsAsync(folderPath, component.NamespaceComponents, projectNames);

        if (!componentUsings.Any())
        {
            return Enumerable.Empty<Violation>().ToList();
        }

        return GetDependencyAnalysisOnComponent(componentUsings, model, component);
    }

    internal static List<Violation> GetDependencyAnalysisOnComponent(
        IList<UsingDirective> usingDirectives,
        ArchitecturalModel model,
        ArchitecturalComponent component)
    {
        var violations = new List<Violation>();
        var dependencyComponents = GetAllDependencyComponents(model, component);

        if (!component.Dependencies.Any())
        {
            return usingDirectives.Select(u => ViolationFactory.CreateDependencyViolation(u, component.Name)).ToList();
        }

        foreach (var directive in usingDirectives)
        {
            if (!dependencyComponents.SelectMany(dep => dep.NamespaceComponents).Any(ns => directive.Using.Contains(ns.Name)))
            {
                violations.Add(ViolationFactory.CreateDependencyViolation(directive, component.Name));
            }
        }

        return violations;
    }

    private static IList<ArchitecturalComponent> GetAllDependencyComponents(ArchitecturalModel model, ArchitecturalComponent component)
    {
        var directComponents = model.Components
            .Where(comp => component.Dependencies
                .Select(dependency => dependency.Id)
                .Contains(comp.Id))
            .ToList();

        AddOpenDependencies(model, directComponents);

        return directComponents.ToList();
    }

    private static void AddOpenDependencies(ArchitecturalModel model, List<ArchitecturalComponent> components)
    {
        var length = components.Count;
        components.AddRange(model.Components
            .Where(comp => components.SelectMany(c => c.Dependencies)
                .Where(dep => dep.IsOpen)
                .Select(dependency => dependency.Id)
                .Contains(comp.Id)));

        components = components.Distinct().ToList();

        if (length != components.Count)
        {
            AddOpenDependencies(model, components);
        }
    }

    private async Task<IList<UsingDirective>> GetUsingsAsync(string folderPath, IList<NamespaceModel> namespaces, IList<string> projectNames)
    {
        List<UsingDirective> usings = new();

        foreach (var n in namespaces)
        {
            var result = await GetUsingDirectivesAsync($"{folderPath}/{n.Name}");
            result.ForEach(u => {
                u.FilePath = u.FilePath.Split($"{folderPath}/")[1];
                u.ComponentName = n.Name;
            });

            usings.AddRange(result);
        }

        return usings
            .Distinct()
            .Where(u => projectNames.Any(proj => u.Using.Contains(proj)) &&
                !namespaces.Any(n => u.Using.Contains(n.Name))
            )
            .ToList();
    }

    private async Task<List<UsingDirective>> GetUsingDirectivesAsync(string folderPath)
    {
        List<UsingDirective> matches = new();
        var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (file.EndsWith(Enum.GetName(typeof(FileExtensions), FileExtensions.cshtml)!) ||
                file.EndsWith(Enum.GetName(typeof(FileExtensions), FileExtensions.cs)!))
            {
                var content = await File.ReadAllLinesAsync(file, Encoding.UTF8);
                var result = content.Where(s => Regex.Match(s, AnalysisRegex.UsingRegex).Success);

                foreach (var match in result)
                {
                    matches.Add(new UsingDirective()
                    {
                        Using = match,
                        File = Path.GetFileName(file),
                        FilePath = file
                    });
                }
            }
        }
        return matches;
    }
}
