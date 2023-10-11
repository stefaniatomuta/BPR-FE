using BPR.Persistence.Models;

namespace BPRBlazor.ViewModels;

public static class Extensions
{
    public static ArchitecturalModelCollection ToBackendModel(this ArchitecturalModelViewModel model)
    {
        return new ArchitecturalModelCollection
        {
            Id = model.Id,
            Name = model.Name,
            Components = model.Components
                .Select(c => c.ToBackendModel())
                .ToList()
        };
    }

    private static ArchitecturalComponentCollection ToBackendModel(this ArchitecturalComponentViewModel component)
    {
        return new ArchitecturalComponentCollection
        {
            Id = component.Id,
            Name = component.Name,
            Dependencies = component.Dependencies
                .Select(c => c.Id)
                .ToList(),
            Position = new Position
            {
                X = component.PositionViewModel.X,
                Y = component.PositionViewModel.Y,
                Height = component.PositionViewModel.Height,
                Width = component.PositionViewModel.Width
            }
        };
    }
    
    public static ArchitecturalModelViewModel ToViewModel(this ArchitecturalModel model)
    {
        var components = model.Components
            .Select(c => c.ToViewModel()).ToList();
        foreach (var component in components)
        {
            component.Dependencies.AddRange(components
                .Where(dependency => model.Components.First(modelComponent => modelComponent.Id == component.Id).Dependencies.Contains(dependency.Id)));
        }

        return new ArchitecturalModelViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Components = components
        };
    }

    private static ArchitecturalComponentViewModel ToViewModel(this ArchitecturalComponent component)
    {
        return new ArchitecturalComponentViewModel
        {
            Id = component.Id,
            Name = component.Name,
            PositionViewModel = new PositionViewModel
            {
                X = component.Position.X,
                Y = component.Position.Y,
                Height = component.Position.Height,
                Width = component.Position.Width
            }
        };
    }
}
