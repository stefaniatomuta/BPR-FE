using BPRBE.Models.Persistence;
using BPRBlazor.Models;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BPRBlazor.Components.ModelManagement;

public partial class CreateEditArchitectureComponent : ComponentBase
{
    [Parameter] 
    public ArchitecturalModelViewModel ModelViewModel { get; set; } = new();
    private List<(string Message, string Class)> _resultMessages = new();
    private ArchitecturalComponentViewModel? _dependencyComponent;
    private Position _dragStartCoordinates = new();
    private ArchitecturalComponentViewModel? _draggingComponent;

    private void AddArchitecturalComponent()
    {
        _dependencyComponent = null;
        _resultMessages = new List<(string Message, string Class)>();

        var component = new ArchitecturalComponentViewModel()
        {
            Id = ModelViewModel.Components.Any() ? ModelViewModel.Components.Max(c => c.Id) + 1 : 0
        };

        ModelViewModel.Components.Add(component);
    }

    private void RemoveArchitecturalComponent(ArchitecturalComponentViewModel component)
    {
        _dependencyComponent = null;
        ModelViewModel.Components.Remove(component);

        foreach (var dependentComponent in ModelViewModel.GetDependentComponents(component))
        {
            RemoveDependency(dependentComponent, component);
        }
    }

    private async Task CreateOrEditArchitectureModel()
    {
        try
        {
            _resultMessages = new();
            var result = await DependencyService.AddOrEditModelAsync(ModelViewModel.ToBackendModel());
            if (result.Success)
            {
               NavigationManager.NavigateTo(NavigationManager.Uri, true);
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
            _dependencyComponent.Dependencies.Add(dependencyComponent);
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
    
    private async Task DeleteSelectedModel()
    {
        var confirmed = await JS.InvokeAsync<bool>("handleConfirmation", new object?[]{$"Are you sure you want to delete the '{ModelViewModel!.Name}' model?"});
        _resultMessages = new List<(string Message, string Class)>();
        
        if (!confirmed)
        {
            return;
        }

        try
        {
            var result = await DependencyService.DeleteArchitectureModelAsync(ModelViewModel.Id);

            if (result.Success)
            {
                NavigationManager.NavigateTo(NavigationManager.Uri, true);
            }
            else
            {
                _resultMessages.Add((result.Errors.First(), "error"));
            }
        }
        catch (Exception ex)
        {
            _resultMessages.Add((ex.Message, "error"));
        }
    }
}
