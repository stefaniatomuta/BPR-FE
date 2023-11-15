using SevenZipExtractor;

namespace BPR.Mediator.Services; 

public class CodebaseService : ICodebaseService {
    private string? _folderPath;

    public string LoadCodebaseInTemp(ArchiveFile file)
    {
        var uploadedRootFolderName = file.Entries.First().FileName;
        _folderPath = ExtractArchive(file);
        return $"{_folderPath}/{uploadedRootFolderName}";
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

        if (Directory.Exists(_folderPath))
        {
            Directory.Delete(_folderPath, true);
        }
    }
}
