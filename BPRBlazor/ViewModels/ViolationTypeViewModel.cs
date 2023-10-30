using BPR.Mediator.Enums;
using BPR.Mediator.Mappers;

namespace BPRBlazor.ViewModels;

public class ViolationTypeViewModel
{
    public ViolationType ViolationType { get; init; }
    public string Name { get; init; }
    public bool IsChecked { get; set; }

    public ViolationTypeViewModel(ViolationType violationType)
    {
        ViolationType = violationType;
        Name = ViolationTypeMapper.GetViolationTypeName(violationType);
        IsChecked = true;
    }
}