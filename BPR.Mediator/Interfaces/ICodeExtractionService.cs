namespace BPR.Mediator.Interfaces;

public interface ICodeExtractionService
{
    /// <summary>
    /// Extracts the projects names within the codebase based on the csproj files
    /// </summary>
    /// <param name="folderPath"> The path to the temp directory where the codebase is stored</param>
    /// <returns>A list with all the project names</returns>
    List<string> GetProjectNames(string folderPath);
}