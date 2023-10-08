namespace BPRBE.Services; 

public interface ICodeExtractionService {
    /// <summary>
    /// Returns all using directives in all the children files of a given path
    /// </summary>
    /// <param name="filepath">path to the parent folder  </param>
    /// <returns>list of using directives</returns>
    List<string> GetUsingDirectives(string filepath);

    /// <summary>
    /// Extracts the projects names within the codebase based on the csproj files
    /// </summary>
    /// <param name="folderPath"> The path to the temp directory where the codebase is stored</param>
    /// <returns>A list with all the project names</returns>
    List<string> GetProjectNames(string folderPath);
}