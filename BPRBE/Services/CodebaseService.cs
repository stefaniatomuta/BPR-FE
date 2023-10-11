﻿using SevenZipExtractor;

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
        var guid = Guid.NewGuid();
        var directory =  Directory.CreateDirectory($"../temp/{guid}");
        var archiveName = archiveFile.Entries.FirstOrDefault()!.FileName;
        archiveFile.Extract(directory.FullName);
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