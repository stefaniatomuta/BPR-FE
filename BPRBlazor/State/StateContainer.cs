using BPRBE.Services.Models;

namespace BPRBlazor.State; 

public class StateContainer {
    private List<ViolationModel>? Violations;
    
    public List<ViolationModel> Property
    {
        get => Violations ?? new List<ViolationModel>();
        set
        {
            Violations = value;
            NotifyStateChanged();
        }
    }
    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}