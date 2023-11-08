using System.Text;
using System.Text.RegularExpressions;
using BPR.Analysis.Models;

namespace BPR.Analysis.Services;

public class CodeExtractionService : ICodeExtractionService
{
    public async Task<List<UsingDirective>> GetUsingDirectives(string folderPath)
    {
        List<UsingDirective> matches = new();
        var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (file.EndsWith(".cs") || file.EndsWith(".cshtml"))
            {
                var content = await File.ReadAllLinesAsync(file,Encoding.UTF8);
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

    public List<string> GetProjectNames(string folderPath)
    {
        List<string> projectNames = new();
        var projDirectories = Directory.GetDirectories(folderPath);
        var files = new List<string>();

        foreach (var dir in projDirectories)
        {
            files.AddRange(Directory.GetFiles(dir)
                 .Where(file => file.EndsWith("csproj")));
        }

        foreach (var file in files)
        {
            projectNames.Add(Path.GetFileName(file).Split(".csproj")[0]);
        }

        return projectNames;
    }

    public async Task<List<NamespaceDirective>> GetNamespaceDirectives(string folderPath)
    {
        List<NamespaceDirective> matches = new();
        var files = Directory.GetFiles(folderPath,"*",SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (file.EndsWith(".cs") || file.EndsWith(".cshtml"))
            {
                var content = await File.ReadAllLinesAsync(file,Encoding.UTF8);
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
}