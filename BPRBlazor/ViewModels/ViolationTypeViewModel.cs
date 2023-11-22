using BPR.Model.Enums;

namespace BPRBlazor.ViewModels;

public class ViolationTypeViewModel
{
    public ViolationType ViolationType { get; init; }
    public string Name { get; init; }
    public bool IsChecked { get; set; }

    public ViolationTypeViewModel(ViolationType violationType)
    {
        ViolationType = violationType;
        Name = Enum.GetName(typeof(ViolationType), violationType) ?? string.Empty;
        IsChecked = true;
    }
}