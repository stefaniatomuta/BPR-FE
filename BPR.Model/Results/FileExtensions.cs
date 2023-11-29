using System.ComponentModel;

namespace BPR.Model.Results;

public enum FileExtensions
{
    [Description(".cshtml")]
    cshtml,

    [Description(".cs")]
    cs,

    [Description(".csproj")]
    csproj
}