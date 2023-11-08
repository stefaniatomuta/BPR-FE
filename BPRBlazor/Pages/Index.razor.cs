using BPR.Analysis.Enums;
using BPR.Analysis.Mappers;
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
    private readonly List<RuleViewModel> _rulesViewModels = new();
    private bool _isAnalysisComplete;
    private LoadingIndicator? _loadingIndicator;

    private void HandleArchitectureModelOnChange(ArchitecturalModel newValue)
    {
        if (_selectedArchitectureViewModel != null)
        {
            foreach (var component in _selectedArchitectureViewModel.Components)
            {
                _unmappedNamespaceComponents.AddRange(component.NamespaceComponents);
            }
        }
        _selectedArchitectureViewModel = newValue.ToViewModel();
        AutoMapNamespaceComponents();
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

    private bool IsDependencyRuleChecked()
    {
        var dependencyRule = _rulesViewModels.FirstOrDefault(rule => AnalysisRuleMapper.GetAnalysisRuleEnum(rule.Name) == AnalysisRule.Dependency);
        if (dependencyRule == null) return false;
        return dependencyRule.IsChecked;
    }

    private async Task StartAnalysis()
    {
        _resultMessageCss = "error";

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

        if (IsDependencyRuleChecked())
        {
            if (_selectedArchitectureViewModel is null)
            {
                _resultMessage = "Please select an architectural model";
                return;
            }

            if (_unmappedNamespaceComponents.Any())
            {
                _resultMessage = "Please make sure all namespaces are mapped to a component";
                return;
            }
        }

        try
        {
            _loadingIndicator?.ToggleLoading(true);
            _isAnalysisComplete = false;
            var architecturalModel = Mapper.Map<AnalysisArchitecturalModel>(_selectedArchitectureViewModel);
            var ruleList = _rulesViewModels.Where(rule => rule.IsChecked)
                .Select(rule => AnalysisRuleMapper.GetAnalysisRuleEnum(rule.Name))
                .ToList();

            var violations = await AnalysisService.GetAnalysisAsync(_folderPath, architecturalModel, ruleList);

            await ProtectedLocalStore.SetAsync("violations", Mapper.Map<List<ViolationModel>>(violations));
            _resultMessage = "The analysis is ready!";
            _resultMessageCss = "success";
            _loadingIndicator?.ToggleLoading(false);
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
        _rulesViewModels.ForEach(rule => rule.IsChecked = false);
        if (IsDependencyRuleChecked()) await JS.InvokeVoidAsync("removeSelectedElement", "selectArchitecture");
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

        if (_allowedFileTypes.All(e => e != fileExtension))
        {
            _resultMessage = $"The uploaded file needs to be one of the following types: '{string.Join(", ", _allowedFileTypes)}'";
            return;
        }

        _loadingIndicator?.ToggleLoading(true);

        try
        {
            await LoadCodebaseAsync(eventArgs.File);
            SetNamespaceComponents();
            AutoMapNamespaceComponents();
            _uploadedFile = eventArgs.File;
        }
        catch (Exception e)
        {
            _folderPath = string.Empty;
            _resultMessage = e.Message;
            CodebaseService.Dispose();
        }

        _loadingIndicator?.ToggleLoading(false);
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

    private void AutoMapNamespaceComponents()
    {
        foreach (var namespaceComponent in _unmappedNamespaceComponents)
        {
            var autoMappingComponent = _selectedArchitectureViewModel?.Components
                .FirstOrDefault(component => namespaceComponent.Name.ToLower().Contains(component.Name.ToLower()));
            
            if (autoMappingComponent != null)
            {
                autoMappingComponent.NamespaceComponents.Add(namespaceComponent);
            }
        }
        
        _unmappedNamespaceComponents.RemoveAll(namespaceComponent => _selectedArchitectureViewModel?.Components
            .SelectMany(component => component.NamespaceComponents).Contains(namespaceComponent) ?? false);
    }

    private void ShowAnalysisResults()
    {
        NavigationManager.NavigateTo("/results");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        CodebaseService.Dispose();
    }
}
