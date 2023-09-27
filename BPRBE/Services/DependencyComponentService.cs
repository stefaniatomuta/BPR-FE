using System.Text.RegularExpressions;
using SevenZipExtractor;

namespace BPRBE.Services; 

public class DependencyComponentService : IDependencyComponentService {

    private ICodebaseService _codebaseService;

    public DependencyComponentService(ICodebaseService codebaseService) {
        _codebaseService = codebaseService;
    }

    public List<string> GetFolderNamesForProjects(string folderPath) {
        var projectNames = Directory.EnumerateDirectories(folderPath);
        var folders = new List<string>();
        foreach (var project in projectNames) {
            var fd = Directory.EnumerateDirectories(project);
            folders.AddRange(fd.Select(f => RemoveFolderPath(folderPath, f)).ToList());
        }
        return folders;
    }

    private string RemoveFolderPath(string folderPath, string pathToClean) {
        return pathToClean.Replace($"{folderPath}\\", "");
    }

    public List<string> GetProjectNamesFromSolution(string folderPath) {
        var projectNames = Directory.EnumerateDirectories(folderPath);
        return projectNames.Select(m=> RemoveFolderPath(folderPath,m)).ToList(); 
        
        // var sln = tempDirectory!.GetFiles("*.sln", SearchOption.AllDirectories).FirstOrDefault()!.FullName;
        // var projNames = GetProjectNames(sln);
        // Directory.Delete(tempDirectory.Name,true);
        // return projNames.OrderBy(p => p).ToList();
    }

    private List<string> GetProjectNames(string path) {
        string projectPattern = @"Project\(""\{[0-9A-Fa-f\-]+\}""\) = ""([^""]+)"", ""([^""]+)"", ""\{[0-9A-Fa-f\-]+\}""";
        Regex regex = new Regex(projectPattern);
        string solutionFileContent = File.ReadAllText(path);
        MatchCollection matches = regex.Matches(solutionFileContent);
        return matches.Select(m => m.Groups[1].Value).ToList();
    }
}