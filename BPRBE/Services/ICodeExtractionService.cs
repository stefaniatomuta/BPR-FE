namespace BPRBE.Services; 

public interface ICodeExtractionService {
    /// <summary>
    /// Returns all using directives in all the children files of a given path
    /// </summary>
    /// <param name="filepath">path to the parent folder  </param>
    /// <returns>list of using directives</returns>
    List<string> GetUsingDirectives(string filepath);
}