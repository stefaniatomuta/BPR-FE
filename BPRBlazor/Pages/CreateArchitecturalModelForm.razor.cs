namespace BPRBlazor.Pages;

public partial class CreateArchitecturalModelForm : ComponentBase
{
    private ArchitecturalModel _model = new();
    private string _resultMessage = "";
    private string _createModelResultCss = "";

    private string _dependencyString = "";

    private void AddArchitecturalComponent()
    {
        var component = new ArchitecturalComponent()
        {
            Id = _model.Components.Any() ? _model.Components.Max(c => c.Id) + 1 : 0
        };

        _model.Components.Add(component);
    }

    private void RemoveArchitecturalComponent(int componentId)
    {
        var component = _model.Components.First(c => c.Id == componentId);
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
}
