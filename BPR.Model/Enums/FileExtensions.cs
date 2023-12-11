using System.ComponentModel;

namespace BPR.Model.Enums;

public enum FileExtensions
{
    [Description(".cshtml")]
    Cshtml,

    [Description(".cs")]
    Cs,

    [Description(".csproj")]
    Csproj
}