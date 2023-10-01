using System.Text.RegularExpressions;

namespace BPRBE.Services; 

public class CodeExtractionService : ICodeExtractionService {
    private string usingRegex = @"^using\s+[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z_][A-Za-z0-9_]*)*;$";
    private string namespaceRegex = @"^\s*namespace\s+[A-Za-z_][A-Za-z0-9_.]*\s*";
    
    public List<string> GetUsingDirectives(string filepath) {
        List<string> matches = new ();
        var files  = Directory.EnumerateFiles(filepath,SearchOption.AllDirectories.ToString());
        foreach (var file in files) {
            if (file.EndsWith(".cs")) {
                var content = File.ReadAllText(file);
                var result = Regex.Matches(content, usingRegex);
                matches.AddRange(result.Select(r =>r.Value));
            }
        }
        return matches;
    }
}