using BPRBE.Models.Persistence;

namespace BPRBlazor.ViewModels;

public static class Extensions
{
    public static ArchitecturalModel ToBackendModel(this ArchitecturalModelViewModel model)
    {
        return new ArchitecturalModel
        {
            Name = model.Name,
            Components = model.Components
                .Select(c => c.ToBackendModel())
                .ToList()
        };
    }

    public static ArchitecturalComponent ToBackendModel(this ArchitecturalComponentViewModel component)
    {
        return new ArchitecturalComponent
        {
            Id = component.Id,
            Name = component.Name,
            Dependencies = component.Dependencies
                .Select(c => c.Id)
                .ToList()
        };
    }
}
