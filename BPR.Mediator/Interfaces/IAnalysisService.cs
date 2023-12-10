using BPR.Model.Architectures;
using BPR.Model.Enums;
using BPR.Model.Results;

namespace BPR.Mediator.Interfaces;

public interface IAnalysisService
{
    /// <summary>
    /// Takes an architecture model and the path to the temp folder where the temp repository is stored and analyses the rule types set on the
    /// A violation of dependency is a using directive that does match not any of the dependencies related to the component
    /// </summary>
    /// <param name="folderPath">The path to the root temp directory</param>
    /// <param name="model">Architecture model </param>\
    /// <param name="ruleTypes">List of violation times as enums </param>\
    /// <returns>A list of all violations and their severity</returns>
    Task<List<Violation>> GetAnalysisAsync(string folderPath, ArchitectureModel model,
        List<RuleType> ruleTypes);
}