﻿using BPRBE.Models.Persistence;
using MongoDB.Driver.Linq;

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
            Dependencies = component.Dependencies
                .Select(c => c.Id)
                .ToList()
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
            Name = component.Name
        };
    }
}
