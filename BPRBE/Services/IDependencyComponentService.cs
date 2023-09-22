using SevenZipExtractor;

namespace BPRBE.Services; 

public interface IDependencyComponentService {

    public List<string> LoadComponentsFromStream(ArchiveFile file);
    public string LoadCodebaseInTemp(ArchiveFile file);
    public List<string> GetProjectNamesFromSolution();
}