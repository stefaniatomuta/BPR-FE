using SevenZipExtractor;

namespace BPRBE.Services; 

public class CodebaseService : ICodebaseService {
    private string folderPath = null!;

    public string LoadCodebaseInTemp(ArchiveFile file) {
        var tempfolder  = ExtractArchive(file);
        try {
            return tempfolder;
        }
        catch (Exception e) {
            throw new Exception(e.Message);
        }
    }
    
    private string ExtractArchive(ArchiveFile archiveFile) {
        var directory =  Directory.CreateDirectory("temp");
        var archiveName = archiveFile.Entries.FirstOrDefault()!.FileName;
        archiveFile.Extract(directory.Name);
        folderPath = directory.FullName;
        return $"{directory.FullName}/{archiveName}";
    }

    public void Dispose() {
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
    }

}