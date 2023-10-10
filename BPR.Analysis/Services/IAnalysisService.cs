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
    /// <summary>
    /// Takes the path to the temp folder where the temp repository is stored and analyses the namespaces in the files against their respective paths from the project root
    /// If a namespace does not match the path then it is a violation
    /// </summary>
    /// <param name="folderPath">The path to the temp repository</param>
    /// <returns>A list of all violations. If there are none the list will be empty</returns>
    List<Violation> GetNamespaceAnalysis(string folderPath);
}