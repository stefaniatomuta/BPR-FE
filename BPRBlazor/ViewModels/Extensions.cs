using BPR.Persistence.Models;

namespace BPRBlazor.ViewModels;

public static class Extensions
{
    public static ArchitecturalModelCollection ToBackendModel(this ArchitecturalModelViewModel model)
    {
        return new ArchitecturalModelCollection
        {
            Name = model.Name,
            Components = model.Components
                .Select(c => c.ToBackendModel())
                .ToList()
        };
    }

    public static ArchitecturalComponentCollection ToBackendModel(this ArchitecturalComponentViewModel component)
    {
        return new ArchitecturalComponentCollection
        {
            Id = component.Id,
            Name = component.Name,
            Dependencies = component.Dependencies
                .Select(c => c.Id)
                .ToList()
        };
    }
}
