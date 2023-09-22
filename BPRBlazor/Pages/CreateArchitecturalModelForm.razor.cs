using BPRBlazor.Models;

namespace BPRBlazor.Pages;

public partial class CreateArchitecturalModelForm : ComponentBase
{
    private ArchitecturalModel model = new();
    private string resultMessage = "";
    private string createModelResultCss = "";

    private void AddArchitecturalComponent()
    {
        var component = new ArchitecturalComponent();
        model.Components.Add(component);
    }

    private void RemoveArchitecturalComponent(ArchitecturalComponent component)
    {
        var componentToRemove = model.Components.First(c => c.Name == component.Name);
        model.Components.Remove(componentToRemove);
    }

    private void CreateArchitecturalModel()
    {
        try
        {
            // TODO - Actually add the model.
            model = new();
            resultMessage = "Model successfully added!";
            createModelResultCss = "success";
        }
        catch (Exception ex)
        {
            resultMessage = ex.Message;
            createModelResultCss = "error";
        }
    }
}
