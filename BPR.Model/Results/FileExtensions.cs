using System.ComponentModel;

namespace BPR.Analysis.Models;

public enum FileExtensions
{
    [Description(".cshtml")]
    cshtml,
    
    [Description(".cs")]
    cs,
    
    [Description(".csproj")]
    csproj
}