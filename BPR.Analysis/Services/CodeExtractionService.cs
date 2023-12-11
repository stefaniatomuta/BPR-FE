using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Model.Enums;

namespace BPR.Analysis.Services;

public class CodeExtractionService : ICodeExtractionService
{
    public List<string> GetProjectNames(string folderPath)
    {
        List<string> projectNames = new();
        var projDirectories = Directory.GetDirectories(folderPath);
        var files = new List<string>();

        foreach (var dir in projDirectories)
        {
            files.AddRange(Directory.GetFiles(dir)
                .Where(file => file.EndsWith(EnumExtensions.GetDescription(FileExtensions.Csproj))));
        }

        foreach (var file in files)
        {
            projectNames.Add(Path.GetFileName(file).Split(EnumExtensions.GetDescription(FileExtensions.Csproj))[0]);
        }

        return projectNames;
    }
}