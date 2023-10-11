using BPRBlazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BPRBlazor.Pages;

public partial class CreateArchitecturalModelForm : ComponentBase
{
    private ArchitecturalModel _model = new();
    private List<(string Message, string Class)> _resultMessages = new();

    private ArchitecturalComponent? _dependencyComponent;
    private Position _dragStartCoordinates = new();
    private ArchitecturalComponent? _draggingComponent;

    private void AddArchitecturalComponent()
    {
        _dependencyComponent = null;

        var component = new ArchitecturalComponent()
        {
            Id = _model.Components.Any() ? _model.Components.Max(c => c.Id) + 1 : 0
        };

        _model.Components.Add(component);
    }

    private void RemoveArchitecturalComponent(ArchitecturalComponent component)
    {
        _dependencyComponent = null;
        _model.Components.Remove(component); 
        
        var dependentComponents = _model.Components.
            Where(dependentComponent => dependentComponent.Dependencies.
                Any(dependency => component == dependency));
        
        foreach (var dependentComponent in dependentComponents)
        {
            dependentComponent.Dependencies.Remove(component);
        }
    }

    private async Task CreateArchitecturalModel()
    {
        try
        {
            _resultMessages = new();
            var result = await repository.AddModelAsync(_model.ToBackendModel());
            if (result.Success)
            {
                _model = new();
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

    private void AddDependency(ArchitecturalComponent dependencyComponent)
    {
        if (_dependencyComponent != null)
        {
            if (_dependencyComponent == dependencyComponent)
            {
                _dependencyComponent = null;
                return;
            }
            if (_dependencyComponent.Dependencies.All(c => c.Id != dependencyComponent.Id))
            {
                _dependencyComponent.Dependencies.Add(_model.Components.First(c => c.Id == dependencyComponent.Id));
            }
            _dependencyComponent = null;
        }
        else
        {
            _dependencyComponent = dependencyComponent;
        }
    }

    private void OnDragComponentStart(DragEventArgs args, ArchitecturalComponent component)
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
        
        var offset = await JS.InvokeAsync<Position>("getElementOffset", _draggingComponent.Id);
        _draggingComponent.Position = new Position()
        {
            X = offset.X + difference.X,
            Y = offset.Y + difference.Y,
            Height = offset.Height,
            Width = offset.Width
        };
    }
}
