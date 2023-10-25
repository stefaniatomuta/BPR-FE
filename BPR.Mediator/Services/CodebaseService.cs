using SevenZipExtractor;

namespace BPR.Mediator.Services; 

public class CodebaseService : ICodebaseService {
    private string? folderPath;

    public string LoadCodebaseInTemp(ArchiveFile file)
    {
        var uploadedRootFolderName = file.Entries.First().FileName;
        folderPath = ExtractArchive(file);
        return $"{folderPath}/{uploadedRootFolderName}";
    }
    
    private static string ExtractArchive(ArchiveFile file)
    {
        var guid = Guid.NewGuid();
        var directory = Directory.CreateDirectory($"../temp/{guid}");
        file.Extract(directory.FullName);
        return directory.FullName;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
    }
}
