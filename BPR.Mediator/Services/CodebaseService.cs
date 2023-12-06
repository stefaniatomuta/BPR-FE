using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using SevenZipExtractor;

namespace BPR.Mediator.Services;

public class CodebaseService : ICodebaseService
{
    private string _folderPath = string.Empty;

    public string LoadCodebaseInTemp(ArchiveFile file)
    {
        var uploadedRootFolderName = file.Entries.First().FileName;
        _folderPath = ExtractArchive(file);
        return $"{_folderPath}\\{uploadedRootFolderName}";
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
        FolderCleanup.Cleanup(_folderPath);
    }
}