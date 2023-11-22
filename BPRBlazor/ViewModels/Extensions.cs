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
            Dependencies = component.Dependencies.Select(dependency => new ArchitecturalComponent()
            {
                Id = dependency.Id,
                IsOpen = dependency.IsOpen
            }).ToList(),
            Position = new Position
            {
                X = component.PositionViewModel.X,
                Y = component.PositionViewModel.Y
            }
        };
    }
    
    public static ArchitecturalModelViewModel ToViewModel(this ArchitecturalModel model)
    {
        var components = model.Components
            .Select(c => c.ToViewModel())
            .OrderBy(c => c.Name)
            .ToList();

        foreach (var component in components)
        {
            var modelComponent = model.Components.First(c => c.Id == component.Id);
            var dependencies = components.Where(dependency => modelComponent.Dependencies
                    .Select(d => d.Id)
                    .Contains(dependency.Id))
                .ToList();

            dependencies.ForEach(dep => dep.IsOpen = modelComponent.Dependencies.First(d => d.Id == dep.Id).IsOpen);
            component.Dependencies.AddRange(dependencies);
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
                Y = component.Position.Y
            }
        };
    }
}
