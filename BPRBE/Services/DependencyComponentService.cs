namespace BPRBE.Services; 

public class DependencyComponentService : IDependencyComponentService {
    
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
    }
}