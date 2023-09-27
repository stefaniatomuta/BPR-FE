namespace BPRBE.Services; 

public interface IDependencyComponentService {

   
    /// <summary>
    /// Gets the project names inside the loaded source code saved in a temp folder
    /// </summary>
    /// <param name="folderPath">The path to the temp folder</param>
    /// <returns>A list with the names of the projects in the solution</returns>
    public List<string> GetProjectNamesFromSolution(string folderPath);

    /// <summary>
    /// Gets all the subfolders from the projects in the codebase
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    public List<string> GetFolderNamesForProjects(string folderPath);
}