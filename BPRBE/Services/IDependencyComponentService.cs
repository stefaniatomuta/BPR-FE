using SevenZipExtractor;

namespace BPRBE.Services; 

public interface IDependencyComponentService {

    /// <summary>
    /// Loads the codebase in a temp folder
    /// </summary>
    /// <param name="file">the source code as a zip file</param>
    /// <returns>the path to the temp folder as a string</returns>
    public string LoadCodebaseInTemp(ArchiveFile file);
    /// <summary>
    /// Gets the project names inside the loaded source code saved in a temp folder
    /// </summary>
    /// <param name="folderPath">The path to the temp folder</param>
    /// <returns>A list with the names of the projects in the solution</returns>
    public List<string> GetProjectNamesFromSolution(string folderPath);

    public List<string> GetFolderNamesForProjects(string folderPath);
}