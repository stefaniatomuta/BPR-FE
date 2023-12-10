using BPR.Analysis.Models;
using BPR.Model.Enums;
using BPR.Model.Results;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using BPR.Mediator.Utils;

namespace BPR.Analysis.Services.Analyses;

internal class NamespaceAnalysis
{
    internal static async Task<List<Violation>> AnalyseAsync(string folderPath)
    {
        var namespaces = await GetNamespaceDirectivesAsync(folderPath);
        return GetNamespaceAnalysis(namespaces, folderPath);
    }

    private static async Task<List<NamespaceDirective>> GetNamespaceDirectivesAsync(string folderPath)
    {
        List<NamespaceDirective> matches = new();
        var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (file.EndsWith(EnumExtensions.GetDescription(FileExtensions.Cshtml)) ||
                file.EndsWith(EnumExtensions.GetDescription(FileExtensions.Cs)))
            {
                var content = await File.ReadAllLinesAsync(file, Encoding.UTF8);
                var result = content.Where(s => Regex.Match(s, AnalysisRegex.NamespaceRegex).Success);

                foreach (var match in result)
                {
                    matches.Add(new NamespaceDirective()
                    {
                        Namespace = match,
                        File = Path.GetFileName(file),
                        FilePath = file
                    });
                }
            }
        }
        return matches;
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
                    Type = RuleType.MismatchedNamespace
                });
            }
        }

        return violations;
    }
}
