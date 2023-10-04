using System.Text;
using System.Text.RegularExpressions;

namespace BPRBE.Services; 

public class CodeExtractionService : ICodeExtractionService {
    private string usingRegex = @"^using\s+[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z_][A-Za-z0-9_]*)*;$";
    private string namespaceRegex = @"^\s*namespace\s+[A-Za-z_][A-Za-z0-9_.]*\s*";
    
    public List<string> GetUsingDirectives(string filepath) {
        List<string> matches = new ();
        var files  = Directory.GetFiles(filepath).ToList();
        foreach (var file in files) {
            if (file.EndsWith(".cs") || file.EndsWith(".cshtml")) {
                var content = File.ReadAllText(file,Encoding.UTF8);
                var result = Regex.Matches(content, usingRegex,RegexOptions.Multiline);
                matches.AddRange(result.Select(r =>r.Value));
            }
        }
        return matches;
    }
}