namespace BPR.Mediator.Services; 

public class DependencyComponentService : IDependencyComponentService
{
    private static readonly string[] IgnoredFolders = new[] { "bin", "obj", ".git", ".github", ".vs", ".Test", ".Tests" };
    private static readonly string[] RequiredFileExtensions = new[] { ".cs", ".cshtml" };

    public IList<string> GetFolderNamesForProjects(string folderPath)
    {
        return Directory
            .EnumerateDirectories(folderPath)
            .Where(IsNotIgnoredFolder)
            .Where(DoesContainCSharpFiles)
            .Select(projectFolderPath => RemoveFolderPath(folderPath, projectFolderPath))
            .ToList();
    }

    private static Func<string, bool> IsNotIgnoredFolder => folderPath => 
        !IgnoredFolders.Any(folderPath.EndsWith);

    private static Func<string, bool> DoesContainCSharpFiles => folderPath =>
        Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)
                 .Any(file => RequiredFileExtensions.Any(file.EndsWith));

    private static string RemoveFolderPath(string folderPath, string pathToClean)
    {
        return pathToClean.Replace($"{folderPath}\\", "");
    }
}
