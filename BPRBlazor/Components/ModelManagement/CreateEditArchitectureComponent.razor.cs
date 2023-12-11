using BPR.Model.Architectures;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BPRBlazor.Components.ModelManagement;

public partial class CreateEditArchitectureComponent : ComponentBase
{
    [Parameter, EditorRequired] 
    public ArchitectureModelViewModel ModelViewModel { get; set; } = new();

    [Parameter]
    public bool IsEditable { get; set; }

    private List<(string Message, string Class)> _resultMessages = new();
    private ArchitectureComponentViewModel? _dependencyComponent;
    private PositionViewModel _dragStartCoordinates = new();
    private ArchitectureComponentViewModel? _draggingComponent;
    private SizeViewModel? _boundarySize;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var update = false;
        var boundarySize = await JS.InvokeAsync<SizeViewModel>("getElementSize", new object[] { "boundary" });

        if (_boundarySize == null)
        {
            _boundarySize = boundarySize;
            update = true;
        }
        else if (boundarySize.Width != _boundarySize.Width)
        {
            _boundarySize = boundarySize;
            update = true;
        }

        if (_boundarySize != null)
        {
            foreach (var component in ModelViewModel.Components.ToList())
            {
                component.Size = await JS.InvokeAsync<SizeViewModel>("getElementSize", new object[] { component.Id });

                if (component.Position.X + component.Size.Width > _boundarySize.Width)
                {
                    component.Position.X = _boundarySize.Width - component.Size.Width - 20;
                    update = true;
                }
            }
        }

        if (update || firstRender)
        {
            StateHasChanged();
        }
    }

    private void AddArchitectureComponent()
    {
        if (!IsEditable)
        {
            return;
        }

        _dependencyComponent = null;
        _resultMessages = new List<(string Message, string Class)>();

        var component = new ArchitectureComponentViewModel()
        {
            Id = ModelViewModel.Components.Any() ? ModelViewModel.Components.Max(c => c.Id) + 1 : 0
        };

        ModelViewModel.Components.Add(component);
    }

    private void RemoveArchitectureComponent(ArchitectureComponentViewModel component)
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

    private async Task CreateOrEditArchitectureModelAsync()
    {
        try
        {
            _resultMessages = new();
            var result = await ArchitectureModelService.AddOrEditModelAsync(Mapper.Map<ArchitectureModel>(ModelViewModel));
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

    private void AddDependency(ArchitectureComponentViewModel dependencyComponent)
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
    
    private void RemoveDependency(ArchitectureComponentViewModel component, ArchitectureComponentViewModel dependency)
    {
        component.Dependencies.RemoveAll(dep => dep.Id == dependency.Id);
    }

    private void ToggleOpenness(DependencyViewModel dependency)
    {
        dependency.IsOpen = !dependency.IsOpen;
    }

    private string ComponentDependencyTypeClass(DependencyViewModel dependency) => dependency.IsOpen ? "btn-success" : "btn-danger";

    private void OnDragComponentStart(DragEventArgs args, ArchitectureComponentViewModel component)
    {
        _dragStartCoordinates = new PositionViewModel()
        {
            X = (int)args.ClientX,
            Y = (int)args.ClientY
        };
        _draggingComponent = component;
    }

    private async Task OnDropComponentAsync(DragEventArgs args)
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

        if (offset.X + difference.X > 0 && offset.Y + difference.Y > 0)
        {
            _draggingComponent.Position = new PositionViewModel()
            {
                X = offset.X + difference.X,
                Y = offset.Y + difference.Y,
            };
        }
    }

    private PositionViewModel GetMiddlePositionForComponent(ArchitectureComponentViewModel component)
    {
        if (component.Size == null)
        {
            return new PositionViewModel();
        }
        
        return new PositionViewModel()
        {
            X = component.Position.X + component.Size.Width / 2,
            Y = component.Position.Y + component.Size.Height / 2
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
    
    private async Task DeleteSelectedModelAsync()
    {
        var confirmed = await JS.InvokeAsync<bool>("handleConfirmation", new object?[]{$"Are you sure you want to delete the '{ModelViewModel.Name}' model?"});
        _resultMessages = new List<(string Message, string Class)>();
        
        if (!confirmed)
        {
            return;
        }

        try
        {
            var result = await ArchitectureModelService.DeleteArchitectureModelAsync(ModelViewModel.Id);

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
