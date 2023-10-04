using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Pages;

public partial class CreateArchitecturalModelForm : ComponentBase
{
    private ArchitecturalModelViewModel _modelViewModel = new();
    private string _resultMessage = "";
    private string _createModelResultCss = "";

    private string _dependencyString = "";

    private void AddArchitecturalComponent()
    {
        var component = new ArchitecturalComponentViewModel()
        {
            Id = _modelViewModel.Components.Any() ? _modelViewModel.Components.Max(c => c.Id) + 1 : 0
        };

        _modelViewModel.Components.Add(component);
    }

    private void RemoveArchitecturalComponent(int componentId)
    {
        var component = _modelViewModel.Components.First(c => c.Id == componentId);
        _modelViewModel.Components.Remove(component);
    }

    private void CreateArchitecturalModel()
    {
        try
        {
            // TODO - Actually add the model.
            Console.WriteLine(_modelViewModel.ToString());
            _modelViewModel = new();
            _resultMessage = "Model successfully added!";
            _createModelResultCss = "success";
        }
        catch (Exception ex)
        {
            _resultMessage = ex.Message;
            _createModelResultCss = "error";
        }
    }

    private void AddDependency(int parentComponentId, int dependencyComponentId)
    {
        _dependencyString = "";

        if (parentComponentId == dependencyComponentId)
        {
            return;
        }

        var parentComponent = _modelViewModel.Components.First(c => c.Id == parentComponentId);

        if (!parentComponent.Dependencies.Any(c => c.Id == dependencyComponentId))
        {
            parentComponent.Dependencies.Add(_modelViewModel.Components.First(c => c.Id == dependencyComponentId));
        }
    }
}
