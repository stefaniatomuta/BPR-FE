using BPR.Persistence.Models;
using BPRBlazor.ViewModels;
using BPR.Analysis.Models;
using BPRBE.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SevenZipExtractor;
using BE = BPRBE.Models.Persistence;

namespace BPRBlazor.Pages;

public partial class Index : ComponentBase
{
    private string _errorMessage = string.Empty;
    private string _folderPath = string.Empty;
    private string _analysisMessage = string.Empty;
    private ArchitecturalModelViewModel _architecturalModelViewModel = default!;
    private List<NamespaceViewModel> _unmappedNamespaceComponents = new();
    private NamespaceViewModel _selectedNamespaceViewModelComponent = default!;
    private ArchitecturalModel _selectedArchitectureModelCollection = default!;
    public List<RuleViewModel> _rulesViewModels = new();

    private List<Violation> violations = new();
    

    private async Task SendDataAsync()
    {
        await HttpService.PostAsync("http://127.0.0.1:8000/post?item=HelloWorld", string.Empty);
    }

    protected override void OnInitialized()
    {
        _errorMessage = default!;
        _architecturalModelViewModel = default!;
        StateContainer.OnChange += StateHasChanged;
    }

    private void HandleArchitectureModelOnChange(ArchitecturalModel newValue)
    {
        _selectedArchitectureModelCollection = newValue;
        // TODO - Actually do something with the selected modelCollection when analysis is started.
    }

    private void HandleRule(RuleViewModel value)
    {
        var index = _rulesViewModels.FindIndex(x => x.Name.Equals(value.Name));
        if (index != -1)
        {
            _rulesViewModels[index] = value;
        }
        else
        {
            _rulesViewModels.Add(value);
        }
    }

    private async Task StartAnalysis()
    {
        if (_selectedArchitectureModelCollection == null)
        {
            _errorMessage = "Analysis cannot start without a selected architectural modelCollection";
        }
        if (_unmappedNamespaceComponents.Any())
        {
            _errorMessage = "Analysis can not start while there are unmapped namespaces";
            return;
        }
        if (_rulesViewModels.All(r => !r.IsChecked))
        {
            _errorMessage = "Analysis can not start without any rule selected";
            return;
        }

        try {
            var architecturalModel = Mapper.Map<AnalysisArchitecturalModel>(_architecturalModelViewModel);
            violations.AddRange(AnalysisService.GetNamespaceAnalysis(_folderPath));
            violations.AddRange(AnalysisService.GetDependencyAnalysis(_folderPath, architecturalModel));
            StateContainer.Property = Mapper.Map<List<ViolationModel>>(violations);
            _analysisMessage = "The analysis is ready. Check out the results page";

        }
        catch (Exception e) {
            _errorMessage = "Oops.. An error occured";
        }

        
    }

    private void HandleDrop(ArchitecturalComponentViewModel componentViewModel = default!)
    {
        var oldComponent = _architecturalModelViewModel.Components.FirstOrDefault(architecturalComponent =>
            architecturalComponent.NamespaceComponents.Contains(_selectedNamespaceViewModelComponent)) ?? default!;
        if (oldComponent != default!)
        {
            oldComponent.NamespaceComponents.Remove(_selectedNamespaceViewModelComponent);
        }

        if (componentViewModel != default!)
        {
            componentViewModel.NamespaceComponents.Add(_selectedNamespaceViewModelComponent);
            _unmappedNamespaceComponents.Remove(_selectedNamespaceViewModelComponent);
        }
        else
        {
            _unmappedNamespaceComponents.Add(_selectedNamespaceViewModelComponent);
        }
    }

    private void HandleDragStart(NamespaceViewModel namespaceViewModelComponent)
    {
        _selectedNamespaceViewModelComponent = namespaceViewModelComponent;
    }

    private async Task LoadCodeSource(InputFileChangeEventArgs eventArgs)
    {
        OnInitialized();
        if (!eventArgs.File.Name.EndsWith(".7z"))
        {
            _errorMessage = "The file uploaded needs to be a .7z type";
            return;
        }

        try
        {
            await LoadCodebaseAsync(eventArgs.File);
            SetNamespaceComponents();
        }
        catch (Exception e)
        {
            _unmappedNamespaceComponents = new();
            _folderPath = string.Empty;
            _errorMessage = e.Message + " " + e.StackTrace;
            CodebaseService.Dispose();
        }
    }

    private async Task LoadCodebaseAsync(IBrowserFile file)
    {
        await using var memoryStream = new MemoryStream();
        await file.OpenReadStream(Int32.MaxValue).CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        using var archiveFile = new ArchiveFile(memoryStream, SevenZipFormat.SevenZip);
        _folderPath = CodebaseService.LoadCodebaseInTemp(archiveFile);
    }

    private void SetNamespaceComponents()
    {
        _unmappedNamespaceComponents = new List<NamespaceViewModel>();
        var folderNames = DependencyComponentService.GetFolderNamesForProjects(_folderPath);
        for (var id = 0; id < folderNames.Count(); id++)
        {
            _unmappedNamespaceComponents.Add(new NamespaceViewModel(id, folderNames[id]));
        }
    }

    public void Dispose()
    {
        CodebaseService.Dispose();
        StateContainer.OnChange -= StateHasChanged;
    }
    
}