using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Model.Enums;

namespace BPR.Mediator.Services;

public class DependencyComponentService : IDependencyComponentService
{
    private static readonly string[] IgnoredFolders = {"bin", "obj", ".git", ".github", ".vs", ".Test", ".Tests"};
    private static readonly string[] RequiredFileExtensions =
    {
        EnumExtensions.GetDescription(FileExtensions.Cs), 
        EnumExtensions.GetDescription(FileExtensions.Cshtml)
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

    private readonly Func<string, bool> IsNotIgnoredFolder = (folderPath) =>
        !IgnoredFolders.Any(folderPath.EndsWith);

    private readonly Func<string, bool> DoesContainCSharpFiles = (folderPath) =>
        Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)
            .Any(file => RequiredFileExtensions.Any(file.EndsWith));

    private string RemoveFolderPath(string folderPath, string pathToClean)
    {
        return pathToClean.Replace($"{folderPath}\\", "");
    }
}