using BPRBlazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BPRBlazor.Pages;

public partial class CreateArchitecturalModelForm : ComponentBase
{
    private ArchitecturalModel _model = new();
    private string _resultMessage = "";
    private string _createModelResultCss = "";

    private string _dependencyString = "";

    private (double ClientX, double ClientY) _dragStartCoordinates;
    private ArchitecturalComponent? _draggingComponent;

    private void AddArchitecturalComponent()
    {
        var component = new ArchitecturalComponent()
        {
            Id = _model.Components.Any() ? _model.Components.Max(c => c.Id) + 1 : 0
        };

        _model.Components.Add(component);
    }

    private void RemoveArchitecturalComponent(ArchitecturalComponent component)
    {
        _model.Components.Remove(component);
    }

    private void CreateArchitecturalModel()
    {
        try
        {
            // TODO - Actually add the model.
            Console.WriteLine(_model.ToString());
            _model = new();
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

        var parentComponent = _model.Components.First(c => c.Id == parentComponentId);

        if (!parentComponent.Dependencies.Any(c => c.Id == dependencyComponentId))
        {
            parentComponent.Dependencies.Add(_model.Components.First(c => c.Id == dependencyComponentId));
        }
    }

    private void OnDragComponentStart(DragEventArgs args, ArchitecturalComponent component)
    {
        _dragStartCoordinates = (args.ClientX, args.ClientY);
        _draggingComponent = component;
    }

    private async Task OnDropComponent(DragEventArgs args)
    {
        if (_draggingComponent == null)
        {
            return;
        }

        var difference = (args.ClientX - _dragStartCoordinates.ClientX, args.ClientY - _dragStartCoordinates.ClientY);
        var offset = await JS.InvokeAsync<HTMLElementOffset>("getElementOffset", _draggingComponent.Id);
        _draggingComponent.Style = $"left: {offset.Left + difference.Item1}px; top: {offset.Top + difference.Item2}px";
    }

    private class HTMLElementOffset
    {
        public double Left { get; set; }
        public double Top { get; set; }
    }
}
