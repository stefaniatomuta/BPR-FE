using System.Text.RegularExpressions;
using SevenZipExtractor;
using SearchOption = Microsoft.VisualBasic.FileIO.SearchOption;

namespace BPRBE.Services; 

public class DependencyComponentService : IDependencyComponentService {
    
    
    public string LoadCodebaseInTemp(ArchiveFile file) {
        var tempDirectory = ExtractArchive(file);
        try {
            return tempDirectory;
        }
        catch (Exception e) {
            Directory.Delete(tempDirectory,true);
            throw new Exception(e.Message);
        }
    }

    public List<string> GetProjectNamesFromSolution(string folderPath) {
        var projectNames = Directory.GetDirectories(folderPath);
        // var sln = tempDirectory!.GetFiles("*.sln", SearchOption.AllDirectories).FirstOrDefault()!.FullName;
        // var projNames = GetProjectNames(sln);
        // Directory.Delete(tempDirectory.Name,true);
        // return projNames.OrderBy(p => p).ToList();
        return projectNames.ToList();
    }

    //step 1: get list of folders and projects
    //how?: from sln read the projects?
    //from each project the folder: project/foldername
    // public List<string> LoadComponentsFromStream(ArchiveFile file) {
    //     var entries = file.Entries;
    //     List<string> folders = new List<string>();
    //     
    // }

    private string ExtractArchive(ArchiveFile archiveFile) {
        var directory =  Directory.CreateDirectory("temp");
        archiveFile.Extract(directory.Name);
        return directory.FullName;
    }

    private List<string> GetProjectNames(string path) {
        string projectPattern = @"Project\(""\{[0-9A-Fa-f\-]+\}""\) = ""([^""]+)"", ""([^""]+)"", ""\{[0-9A-Fa-f\-]+\}""";
        Regex regex = new Regex(projectPattern);
        string solutionFileContent = File.ReadAllText(path);
        MatchCollection matches = regex.Matches(solutionFileContent);
        return matches.Select(m => m.Groups[1].Value).ToList();

    }
}