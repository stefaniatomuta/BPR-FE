using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Model.Enums;
using SevenZipExtractor;

namespace BPR.Mediator.Services;

public class CodebaseService : ICodebaseService
{
    private string _folderPath = string.Empty;

    public string LoadCodebaseInTemp(ArchiveFile file)
    {
        var uploadedRootFolderName = file.Entries.First().FileName.Split("\\")[0];
        _folderPath = ExtractArchive(file);
        return $"{_folderPath}\\{uploadedRootFolderName}";
    }

    private static string ExtractArchive(ArchiveFile file)
    {
        var guid = Guid.NewGuid();
        var directory = Directory.CreateDirectory($"../temp/{guid}");

        foreach (var entry in file.Entries.Where(entry =>
             entry.FileName.EndsWith(EnumExtensions.GetDescription(FileExtensions.Cshtml)) ||
             entry.FileName.EndsWith(EnumExtensions.GetDescription(FileExtensions.Csproj)) ||
             entry.FileName.EndsWith(EnumExtensions.GetDescription(FileExtensions.Cs))))
        {
            entry.Extract($"{directory.FullName}\\{entry.FileName}");
        }

        return directory.FullName;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        FolderCleanup.Cleanup(_folderPath);
    }
}
