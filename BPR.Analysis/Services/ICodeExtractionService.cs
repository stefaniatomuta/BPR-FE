using BPR.Analysis.Models;

namespace BPR.Analysis.Services; 

public interface ICodeExtractionService {
    /// <summary>
    /// Returns all using directives in all the children files of a given path
    /// </summary>
    /// <param name="folderPath">path to the parent folder  </param>
    /// <returns>list of using directives</returns>
    Task<List<UsingDirective>> GetUsingDirectives(string folderPath);

    /// <summary>
    /// Extracts the projects names within the codebase based on the csproj files
    /// </summary>
    /// <param name="folderPath"> The path to the temp directory where the codebase is stored</param>
    /// <returns>A list with all the project names</returns>
    List<string> GetProjectNames(string folderPath);

    /// <summary>
    /// Extracts all namespaces in the project
    /// </summary>
    /// <param name="folderPath">The path to the temp directory where the codebase is stored</param>
    /// <returns>A list with all namespaces, the file name of the file they were extracted from and the path to the file</returns>
    Task<List<NamespaceDirective>> GetNamespaceDirectives(string folderPath);
}