using System.Text;
using System.Text.RegularExpressions;
using BPR.Analysis.Models;

namespace BPR.Analysis.Services; 

public class CodeExtractionService : ICodeExtractionService {
    private string usingRegex = @"^using\s+[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z_][A-Za-z0-9_]*)*;$";
    private string namespaceRegex = @"^\s*namespace\s+[A-Za-z_][A-Za-z0-9_.]*\s*";
    
    public List<UsingDirective> GetUsingDirectives(string folderPath) {
        List<UsingDirective> matches = new ();
        var files  = Directory.GetFiles(folderPath).ToList();
        foreach (var file in files) {
            if (file.EndsWith(".cs") || file.EndsWith(".cshtml")) {
                var content = File.ReadLines(file,Encoding.UTF8);
                var result = content.Where(s => Regex.Match(s, usingRegex).Success).ToList();
                foreach (var match in result) {
                    matches.Add(new UsingDirective() {
                        Using = match,
                        File = Path.GetFileName(file),
                        FilePath = file
                    });
                }
            }
        }
        return matches;
    }

    public List<string> GetProjectNames(string folderPath) {
        List<string> projectNames = new();
        var projDirectories = Directory.GetDirectories(folderPath).ToList();
        var files = new List<string>();
        foreach (var dir in projDirectories) {
            files.AddRange(Directory.GetFiles(dir).ToList().Where(file => file.EndsWith("csproj")).ToList());
        }
        foreach (var file in files) {
            projectNames.Add(Path.GetFileName(file).Split(".csproj")[0]);
        }
        return projectNames;
    }

    public List<NamespaceDirective> GetNamespaceDirectives(string folderPath) {
        List<NamespaceDirective> matches = new ();
        var files  = Directory.GetFiles(folderPath,"*",SearchOption.AllDirectories).ToList();
        foreach (var file in files) {
            if (file.EndsWith(".cs") || file.EndsWith(".cshtml")) {
                var content = File.ReadLines(file,Encoding.UTF8);
                var result = content.Where(s => Regex.Match(s, namespaceRegex).Success).ToList();
                foreach (var match in result) {
                    matches.Add(new NamespaceDirective() {
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