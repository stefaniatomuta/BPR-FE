namespace BPR.Mediator.Services; 

public interface IDependencyComponentService
{
    /// <summary>
    /// Gets all the subfolders from the projects in the codebase
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    public IList<string> GetFolderNamesForProjects(string folderPath);
}