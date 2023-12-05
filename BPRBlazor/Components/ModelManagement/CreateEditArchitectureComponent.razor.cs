using BPR.Model.Architectures;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BPRBlazor.Components.ModelManagement;

public partial class CreateEditArchitectureComponent : ComponentBase
{
    [Parameter, EditorRequired] 
    public ArchitecturalModelViewModel ModelViewModel { get; set; } = new();

    [Parameter]
    public bool IsEditable { get; set; }

    private List<(string Message, string Class)> _resultMessages = new();
    private ArchitecturalComponentViewModel? _dependencyComponent;
    private PositionViewModel _dragStartCoordinates = new();
    private ArchitecturalComponentViewModel? _draggingComponent;
    private SizeViewModel? _componentSize;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_componentSize == null && ModelViewModel.Components.Any())
        {
            _componentSize = await JS.InvokeAsync<SizeViewModel>("getElementSizeByClass", new object[]{"component"});
            StateHasChanged();
        }    
    }
    
    private void AddArchitecturalComponent()
    {
        if (!IsEditable)
        {
            return;
        }

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
        if (!IsEditable)
        {
            return;
        }

        _dependencyComponent = null;
        ModelViewModel.Components.Remove(component);

        foreach (var dependentComponent in ModelViewModel.Components)
        {
            dependentComponent.Dependencies.RemoveAll(dependency => dependency.Id == component.Id);
        }
    }

    private async Task CreateOrEditArchitectureModel()
    {
        try
        {
            _resultMessages = new();
            var result = await DependencyService.AddOrEditModelAsync(Mapper.Map<ArchitecturalModel>(ModelViewModel));
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
        if (!IsEditable)
        {
            return;
        }

        if (_dependencyComponent != null)
        {
            if (_dependencyComponent == dependencyComponent)
            {
                _dependencyComponent = null;
                return;
            }
            _dependencyComponent.Dependencies.Add(new DependencyViewModel()
            {
                Id = dependencyComponent.Id,
                IsOpen = true
            });
            _dependencyComponent = null;
        }
        else
        {
            _dependencyComponent = dependencyComponent;
        }
    }
    
    private static void RemoveDependency(ArchitecturalComponentViewModel component, ArchitecturalComponentViewModel dependency)
    {
        component.Dependencies.RemoveAll(dep => dep.Id == dependency.Id);
    }

    private static void ToggleOpenness(DependencyViewModel dependency)
    {
        dependency.IsOpen = !dependency.IsOpen;
    }

    private static string ComponentDependencyTypeClass(DependencyViewModel dependency) => dependency.IsOpen ? "btn-success" : "btn-danger";

    private void OnDragComponentStart(DragEventArgs args, ArchitecturalComponentViewModel component)
    {
        if (!IsEditable)
        {
            return;
        }

        _dragStartCoordinates = new PositionViewModel()
        {
            X = (int)args.ClientX,
            Y = (int)args.ClientY
        };
        _draggingComponent = component;
    }

    private async Task OnDropComponent(DragEventArgs args)
    {
        if (_draggingComponent == null)
        {
            return;
        }

        var difference = new PositionViewModel
        {
            X = (int)args.ClientX - _dragStartCoordinates.X,
            Y = (int)args.ClientY - _dragStartCoordinates.Y
        };
        
        var offset = await JS.InvokeAsync<PositionViewModel>("getElementOffset", new object[]{_draggingComponent.Id});
        _draggingComponent.Position = new PositionViewModel()
        {
            X = offset.X + difference.X,
            Y = offset.Y + difference.Y,
        };
    }

    private PositionViewModel GetMiddlePositionForComponent(ArchitecturalComponentViewModel component)
    {
        if (_componentSize == null)
        {
            return new PositionViewModel();
        }
        
        return new PositionViewModel()
        {
            X = component.Position.X + _componentSize.Width / 2,
            Y = component.Position.Y + _componentSize.Height / 2
        };
    }
    
    private int GetBottomPositionForComponents()
    {
        if (ModelViewModel.Components.Count == 0)
        {
            return 400;
        }
        var bottomComponent = ModelViewModel.Components.MaxBy(c => c.Position.Y);

        return bottomComponent is {Position.Y: > 200}
            ? bottomComponent.Position.Y + 200
            : 400;
    }
    
    private async Task DeleteSelectedModel()
    {
        var confirmed = await JS.InvokeAsync<bool>("handleConfirmation", new object?[]{$"Are you sure you want to delete the '{ModelViewModel.Name}' model?"});
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
