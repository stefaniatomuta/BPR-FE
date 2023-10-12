using SevenZipExtractor;

namespace BPRBE.Services.Services; 

public interface ICodebaseService : IDisposable {

  /// <summary>
  /// Loads the codebase in a temp folder
  /// </summary>
  /// <param name="file">the source code as a zip file</param>
  /// <returns>the path to the temp folder as a string</returns>
  public string LoadCodebaseInTemp(ArchiveFile file);

}