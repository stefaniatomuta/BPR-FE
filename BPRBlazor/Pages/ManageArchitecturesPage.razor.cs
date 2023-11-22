using BPR.Model.Architectures;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Pages;

public partial class ManageArchitecturesPage : ComponentBase
{
    private ArchitecturalModelViewModel? _selectedModel;
    
    private void HandleModelChange(ArchitecturalModel model)
    {
        _selectedModel = Mapper.Map<ArchitecturalModelViewModel>(model);
    }
    
    private void CreateNewArchitecture()
    {
        _selectedModel = new ArchitecturalModelViewModel();
    }
}