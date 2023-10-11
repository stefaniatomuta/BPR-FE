using BPRBE.Models.Persistence;
using BPRBlazor.Models;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BPRBlazor.Components.ModelManagement;

public partial class CreateArchitectureComponent : ComponentBase
{
    private ArchitecturalModelViewModel _modelViewModel = new();
    private List<(string Message, string Class)> _resultMessages = new();
    private ArchitecturalComponentViewModel? _dependencyComponent;
    private Position _dragStartCoordinates = new();
    private ArchitecturalComponentViewModel? _draggingComponent;

    private void AddArchitecturalComponent()
    {
        _dependencyComponent = null;

        var component = new ArchitecturalComponentViewModel()
        {
            Id = _modelViewModel.Components.Any() ? _modelViewModel.Components.Max(c => c.Id) + 1 : 0
        };

        _modelViewModel.Components.Add(component);
    }

    private void RemoveArchitecturalComponent(ArchitecturalComponentViewModel component)
    {
        _dependencyComponent = null;
        _modelViewModel.Components.Remove(component);

        foreach (var dependentComponent in _modelViewModel.GetDependentComponents(component))
        {
            RemoveDependency(dependentComponent, component);
        }
    }

    private async Task CreateArchitectureModel()
    {
        try
        {
            _resultMessages = new();
            var result = await service.AddModelAsync(_modelViewModel.ToBackendModel());
            if (result.Success)
            {
                _modelViewModel = new();
                _resultMessages.Add(("Model successfully added!", "success"));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _resultMessages.Add((error, "error"));
                }
            }
        }
        catch (Exception ex)
        {
            _resultMessages.Add((ex.Message, "error"));
        }
    }

    private void AddDependency(ArchitecturalComponentViewModel dependencyComponent)
    {
        if (_dependencyComponent != null)
        {
            if (_dependencyComponent == dependencyComponent)
            {
                _dependencyComponent = null;
                return;
            }
            if (!_dependencyComponent.HasDependency(dependencyComponent) && !dependencyComponent.HasDependency(_dependencyComponent))
            {
                _dependencyComponent.Dependencies.Add(dependencyComponent);
            }
            _dependencyComponent = null;
        }
        else
        {
            _dependencyComponent = dependencyComponent;
        }
    }
    
    private static void RemoveDependency(ArchitecturalComponentViewModel component, ArchitecturalComponentViewModel dependency)
    {
        component.Dependencies.Remove(dependency);
    }

    private void OnDragComponentStart(DragEventArgs args, ArchitecturalComponentViewModel component)
    {
        _dragStartCoordinates = new Position()
        {
            X = args.ClientX,
            Y = args.ClientY
        };
        _draggingComponent = component;
    }

    private async Task OnDropComponent(DragEventArgs args)
    {
        if (_draggingComponent == null)
        {
            return;
        }

        var difference = new Position
        {
            X = args.ClientX - _dragStartCoordinates.X,
            Y = args.ClientY - _dragStartCoordinates.Y
        };
        
        var offset = await JS.InvokeAsync<Position>("getElementOffset", new object[]{_draggingComponent.Id});
        _draggingComponent.Position = new Position()
        {
            X = offset.X + difference.X,
            Y = offset.Y + difference.Y,
            Height = offset.Height,
            Width = offset.Width
        };
    }
}
