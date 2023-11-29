using BPR.Mediator.Interfaces;
using BPR.Model.Results;

namespace BPR.Mediator.Services;

public class DependencyComponentService : IDependencyComponentService
{
    private static readonly string[] IgnoredFolders = new[] {"bin", "obj", ".git", ".github", ".vs", ".Test", ".Tests"};
    private static readonly string[] RequiredFileExtensions = new[]
    {
        Enum.GetName(typeof(FileExtensions),FileExtensions.cs) ?? string.Empty, 
        Enum.GetName(typeof(FileExtensions),FileExtensions.cshtml) ?? string.Empty
    };

    public IList<string> GetFolderNamesForProjects(string folderPath)
    {
        return Directory
            .EnumerateDirectories(folderPath)
            .Where(IsNotIgnoredFolder)
            .Where(DoesContainCSharpFiles)
            .Select(projectFolderPath => RemoveFolderPath(folderPath, projectFolderPath))
            .ToList();
    }

    private readonly static Func<string, bool> IsNotIgnoredFolder = (folderPath) =>
        !IgnoredFolders.Any(folderPath.EndsWith);

    private readonly static Func<string, bool> DoesContainCSharpFiles = (folderPath) =>
        Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)
            .Any(file => RequiredFileExtensions.Any(file.EndsWith));

    private static string RemoveFolderPath(string folderPath, string pathToClean)
    {
        return pathToClean.Replace($"{folderPath}\\", "");
    }
}