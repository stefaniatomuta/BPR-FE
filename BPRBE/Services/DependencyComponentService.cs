using System.Text.RegularExpressions;
using SevenZipExtractor;

namespace BPRBE.Services; 

public class DependencyComponentService : IDependencyComponentService {
    public string LoadCodebaseInTemp(ArchiveFile file) {
        throw new NotImplementedException();
    }

    public List<string> GetProjectNamesFromSolution() {
        throw new NotImplementedException();
    }

    //step 1: get list of folders and projects
    //how?: from sln read the projects?
    //from each project the folder: project/foldername
    public List<string> LoadComponentsFromStream(ArchiveFile file) {
        var entries = file.Entries;
        List<string> folders = new List<string>();
        var tempDirectory = ExtractArchive(file);
        try {
            var sln = tempDirectory.GetFiles("*.sln", SearchOption.AllDirectories).FirstOrDefault()!.FullName;
            var projNames = GetProjectNames(sln);
            // Console.WriteLine(solutionName);
            foreach (var entry in entries) {
                if (entry.IsFolder) {
                    folders.Add(entry.FileName);
                }
            }

            Directory.Delete(tempDirectory.Name,true);
            return projNames.OrderBy(p => p).ToList();
        }
        catch (Exception e) {
            Directory.Delete(tempDirectory.Name,true);
            throw new Exception(e.Message);
        }
    }

    private DirectoryInfo ExtractArchive(ArchiveFile archiveFile) {
        var directory =  Directory.CreateDirectory("temp");
        archiveFile.Extract(directory.Name);
        
        return directory;
    }

    private List<string> GetProjectNames(string path) {
        string projectPattern = @"Project\(""\{[0-9A-Fa-f\-]+\}""\) = ""([^""]+)"", ""([^""]+)"", ""\{[0-9A-Fa-f\-]+\}""";
        Regex regex = new Regex(projectPattern);
        string solutionFileContent = File.ReadAllText(path);
        MatchCollection matches = regex.Matches(solutionFileContent);
        return matches.Select(m => m.Groups[1].Value).ToList();

    }
}