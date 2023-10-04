namespace BPRBE.Services; 

public class DependencyComponentService : IDependencyComponentService {
    private List<string> foldersToIgnore = new () {"bin","obj" };

    public List<string> GetFolderNamesForProjects(string folderPath) {
        var projectNames = Directory.EnumerateDirectories(folderPath).ToList();
        var result  = projectNames
            .Where(projectName => foldersToIgnore.Any(folder => !projectName.Contains(folder)))
            .ToList();
        var folders = new List<string>();
        foreach (var project in result) {
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