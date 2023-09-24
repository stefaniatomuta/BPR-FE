namespace BPRBlazor.Pages;

public partial class CreateArchitecturalModelForm : ComponentBase
{
    private ArchitecturalModel _model = new();
    private string _resultMessage = "";
    private string _createModelResultCss = "";

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
}
