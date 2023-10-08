using BPR.Analysis.Models;

namespace BPR.Analysis.Services; 

public interface IAnalysisService {
    /// <summary>
    /// Takes an architecture model and compares the list of dependencies for each component with the list of using directives from each cs file in the directive
    /// A violation of dependency is a using directive that does match not any of the dependencies related to the component
    /// </summary>
    /// <param name="folderPath">The path to the root temp directory</param>
    /// <param name="model">Architectural model </param>
    /// <returns>A list of all violations and their severity</returns>
    List<Violation> GetDependencyAnalysis(string folderPath, AnalysisArchitecturalModel model);

    List<Violation> GetNamespaceAnalysis(string folderPath);
}