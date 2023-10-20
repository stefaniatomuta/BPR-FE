using BPR.Analysis.Models;
using BPR.Mediator.Models;
using BPRBlazor.Components.Common;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SevenZipExtractor;

namespace BPRBlazor.Pages;

public partial class Index : ComponentBase
{
    private IBrowserFile? _uploadedFile;
    private readonly string[] _allowedFileTypes = { ".7z", ".zip" };
    private string _folderPath = string.Empty;
    private string _resultMessage = string.Empty;
    private string _resultMessageCss = string.Empty;
    private ArchitecturalModelViewModel? _selectedArchitectureViewModel;
    private List<NamespaceViewModel> _unmappedNamespaceComponents = new();
    private NamespaceViewModel? _selectedNamespaceViewModelComponent;
    private List<RuleViewModel> _rulesViewModels = new();
    private List<Violation> _violations = new();
    private bool _isAnalysisComplete;
    private LoadingIndicator? _loadingIndicator;

    protected override void OnInitialized()
    {
        StateContainer.OnChange += StateHasChanged;
    }

    private void HandleArchitectureModelOnChange(ArchitecturalModel newValue)
    {
        _selectedArchitectureViewModel = newValue.ToViewModel();
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
        _resultMessageCss = "error";
        if (_selectedArchitectureViewModel is null)
        {
            _resultMessage = "Please select an architectural model";
            return;
        }

        if (_uploadedFile is null)
        {
            _resultMessage = "Please upload the source code";
            return;
        }

        if (_rulesViewModels.All(r => !r.IsChecked))
        {
            _resultMessage = "Please select one or more rules to analyse against";
            return;
        }

        if (_unmappedNamespaceComponents.Any())
        {
            _resultMessage = "Please make sure all namespaces are mapped to a component";
            return;
        }

        try
        {
            _loadingIndicator?.ToggleLoading();
            _isAnalysisComplete = false;
            var architecturalModel = Mapper.Map<AnalysisArchitecturalModel>(_selectedArchitectureViewModel);

            // TODO - Would probably make a lot more sense to just call a StartAnalysis method on the AnalysisService.
            // Could then pass all required parameters and have the service figure out what rules to analyse against.
            if (_rulesViewModels.Any(r => r.Name == "Dependency" && r.IsChecked))
            {
                _violations.AddRange(AnalysisService.GetDependencyAnalysis(_folderPath, architecturalModel));
            }

            if (_rulesViewModels.Any(r => r.Name == "Namespace" && r.IsChecked))
            {
                _violations.AddRange(AnalysisService.GetNamespaceAnalysis(_folderPath));
            }

            StateContainer.Property = Mapper.Map<List<ViolationModel>>(_violations);
            _resultMessage = "The analysis is ready!";
            _resultMessageCss = "success";
            _loadingIndicator?.ToggleLoading();
            _isAnalysisComplete = true;
            await Reset();
        }
        catch (Exception)
        {
            _resultMessage = "An error occured...";
            throw;
        }
    }

    private async Task Reset()
    {
        _uploadedFile = null;
        _unmappedNamespaceComponents = new();
        _selectedArchitectureViewModel = null;
        _violations = new();
        await JS.InvokeVoidAsync("removeSelectedElement", "selectArchitecture");
    }

    private void HandleDragStart(NamespaceViewModel namespaceViewModelComponent)
    {
        _selectedNamespaceViewModelComponent = namespaceViewModelComponent;
    }

    private void HandleDrop(ArchitecturalComponentViewModel? componentViewModel = null)
    {
        if (_selectedArchitectureViewModel is null || _selectedNamespaceViewModelComponent is null)
        {
            return;
        }

        var oldComponent = _selectedArchitectureViewModel.Components.FirstOrDefault(architecturalComponent =>
            architecturalComponent.NamespaceComponents.Contains(_selectedNamespaceViewModelComponent));

        oldComponent?.NamespaceComponents.Remove(_selectedNamespaceViewModelComponent);

        if (componentViewModel is not null)
        {
            componentViewModel.NamespaceComponents.Add(_selectedNamespaceViewModelComponent);
            _unmappedNamespaceComponents.Remove(_selectedNamespaceViewModelComponent);
        }
        else
        {
            if (_unmappedNamespaceComponents.Contains(_selectedNamespaceViewModelComponent))
            {
                return;
            }

            _unmappedNamespaceComponents.Add(_selectedNamespaceViewModelComponent);
        }
    }

    private async Task LoadCodeSource(InputFileChangeEventArgs eventArgs)
    {
        var fileExtension = Path.GetExtension(eventArgs.File.Name);
        _resultMessageCss = "error";

        if (!_allowedFileTypes.Any(e => e == fileExtension))
        {
            _resultMessage = $"The uploaded file needs to be one of the following types: {string.Join(", ", _allowedFileTypes)}";
            return;
        }

        _loadingIndicator?.ToggleLoading();

        try
        {
            await LoadCodebaseAsync(eventArgs.File);
            SetNamespaceComponents();
            _uploadedFile = eventArgs.File;
        }
        catch (Exception e)
        {
            _folderPath = string.Empty;
            _resultMessage = e.Message;
            CodebaseService.Dispose();
        }

        _loadingIndicator?.ToggleLoading();
    }

    private async Task LoadCodebaseAsync(IBrowserFile file)
    {
        await using var memoryStream = new MemoryStream();
        await file.OpenReadStream(int.MaxValue).CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        using var archiveFile = new ArchiveFile(memoryStream);
        _folderPath = CodebaseService.LoadCodebaseInTemp(archiveFile);
    }

    private void SetNamespaceComponents()
    {
        var folderNames = DependencyComponentService.GetFolderNamesForProjects(_folderPath);
        _resultMessage = string.Empty;

        if (!folderNames.Any())
        {
            _resultMessageCss = "error";
            _resultMessage = "No projects found in uploaded file...";
            return;
        }

        _unmappedNamespaceComponents.Clear();
        _selectedArchitectureViewModel?.Components.ForEach(component => component.NamespaceComponents.Clear());

        for (var id = 0; id < folderNames.Count; id++)
        {
            _unmappedNamespaceComponents.Add(new NamespaceViewModel(id, folderNames[id]));
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        CodebaseService.Dispose();
        StateContainer.OnChange -= StateHasChanged;
    }
}
