using BPR.Analysis.Enums;
using BPR.Analysis.Models;

namespace BPR.Analysis.Services; 

public interface IAnalysisService {
    /// <summary>
    /// Takes an architecture model and the path to the temp folder where the temp repository is stored and analyses the rules set on the
    /// A violation of dependency is a using directive that does match not any of the dependencies related to the component
    /// </summary>
    /// <param name="folderPath">The path to the root temp directory</param>
    /// <param name="model">Architectural model </param>\
    /// <param name="rules">List of rules as enums </param>\
    /// <returns>A list of all violations and their severity</returns>
    Task<List<Violation>> GetAnalysisAsync(string folderPath, AnalysisArchitecturalModel model, List<AnalysisRule> rules, bool isOpenArchitecture);
}