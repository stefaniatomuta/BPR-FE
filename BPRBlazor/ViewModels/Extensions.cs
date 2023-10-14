using BPR.Persistence.Models;
using BPR.Mediator.Models;

namespace BPRBlazor.ViewModels;

public static class Extensions
{
    public static ArchitecturalModel ToBackendModel(this ArchitecturalModelViewModel model)
    {
        return new ArchitecturalModel
        {
            Id = model.Id,
            Name = model.Name,
            Components = model.Components
                .Select(c => c.ToBackendModel())
                .ToList()
        };
    }

    private static ArchitecturalComponent ToBackendModel(this ArchitecturalComponentViewModel component)
    {
        return new ArchitecturalComponent
        {
            Id = component.Id,
            Name = component.Name,
            Dependencies = component.Dependencies.Select(x =>x.ToBackendModel()).ToList(),
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
            Dependencies = component.Dependencies.Select(x =>x.ToViewModel()).ToList(),
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
