using BPR.Model.Architectures;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Pages;

public partial class ManageArchitecturesPage : ComponentBase
{
    private ArchitectureModelViewModel? _selectedModel;
    
    private void HandleModelChange(ArchitectureModel model)
    {
        _selectedModel = Mapper.Map<ArchitectureModelViewModel>(model);
    }
    
    private void CreateNewArchitecture()
    {
        _selectedModel = new ArchitectureModelViewModel();
    }
}