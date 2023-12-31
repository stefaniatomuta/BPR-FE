using BPR.Model.Architectures;
using BPR.Model.Enums;
using BPR.Model.Rules;
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
    private ArchitectureModelViewModel? _selectedArchitectureViewModel;
    private List<NamespaceViewModel> _unmappedNamespaceComponents = new();
    private NamespaceViewModel? _selectedNamespaceViewModelComponent;
    private readonly List<RuleViewModel> _rulesViewModels = new();
    private LoadingIndicator? _loadingIndicator;
    private bool _isStartAnalysisButtonDisabled;
    private string _analysisTitle = string.Empty;

    private void HandleArchitectureModelOnChange(ArchitectureModel newValue)
    {
        if (_selectedArchitectureViewModel != null)
        {
            foreach (var component in _selectedArchitectureViewModel.Components)
            {
                _unmappedNamespaceComponents.AddRange(component.NamespaceComponents);
            }
        }
        _selectedArchitectureViewModel = Mapper.Map<ArchitectureModelViewModel>(newValue);
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
        var dependencyRule = _rulesViewModels.FirstOrDefault(rule => rule.RuleType == RuleType.ForbiddenDependency);
        if (dependencyRule == null) return false;
        return dependencyRule.IsChecked;
    }

    private async Task StartAnalysisAsync()
    {
        _resultMessageCss = "error";

        if (!IsAnalysisModelValid())
        {
            return;
        }

        try
        {
            var architectureModel = Mapper.Map<ArchitectureModel>(_selectedArchitectureViewModel);
            var ruleList = _rulesViewModels.Where(rule => rule.IsChecked).Select(rule => Mapper.Map<Rule>(rule)).ToList();
            _loadingIndicator?.ToggleLoading(true);
            _isStartAnalysisButtonDisabled = true;
            var result = await ResultService.CreateResultAsync(_folderPath, architectureModel, ruleList, _analysisTitle);
            
            if (result.Success)
            {
                var analysis = result.Value!;
                if (analysis.ResultStatus == ResultStatus.Finished)
                {
                    ToastService.ShowSnackbar(analysis.Id);
                    CodebaseService.Dispose();
                    _resultMessage = string.Empty;
                }
                else
                {
                    _resultMessageCss = "success";
                    _resultMessage = "Analysis started... You will be notified when it is complete";
                }
            }
            else
            {
                _resultMessage = $"Analysis failed... Reason: '{result.Errors.FirstOrDefault()}'";
            }

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
        _isStartAnalysisButtonDisabled = false;
        _loadingIndicator?.ToggleLoading(false);
        _analysisTitle = string.Empty;
        _folderPath = string.Empty;
        if (IsDependencyRuleChecked()) await JS.InvokeVoidAsync("removeSelectedElement", "selectArchitecture");
    }

    private bool IsAnalysisModelValid()
    {
        if (_uploadedFile is null)
        {
            _resultMessage = "Please upload the source code";
            return false;
        }

        if (_rulesViewModels.All(r => !r.IsChecked))
        {
            _resultMessage = "Please select one or more rules to analyse against";
            return false;
        }

        if (IsDependencyRuleChecked())
        {
            if (_selectedArchitectureViewModel is null)
            {
                _resultMessage = "Please select an architecture model";
                return false;
            }

            if (_unmappedNamespaceComponents.Any())
            {
                _resultMessage = "Please make sure all namespaces are mapped to an architecture component";
                return false;
            }
        }

        if (string.IsNullOrWhiteSpace(_analysisTitle))
        {
            _resultMessage = "Please give your analysis a title - for comparison against historical analyses";
            return false;
        }

        return true;
    }

    private void HandleDragStart(NamespaceViewModel namespaceViewModelComponent)
    {
        _selectedNamespaceViewModelComponent = namespaceViewModelComponent;
    }

    private void HandleDrop(ArchitectureComponentViewModel? componentViewModel = null)
    {
        if (_selectedArchitectureViewModel is null || _selectedNamespaceViewModelComponent is null)
        {
            return;
        }

        var oldComponent = _selectedArchitectureViewModel.Components.FirstOrDefault(architectureComponent =>
            architectureComponent.NamespaceComponents.Contains(_selectedNamespaceViewModelComponent));

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
        _resultMessage = string.Empty;
        _resultMessageCss = "error";

        if (_allowedFileTypes.All(e => e != fileExtension))
        {
            _resultMessage = $"The uploaded file needs to be one of the following types: '{string.Join(", ", _allowedFileTypes)}'";
            return;
        }

        _loadingIndicator?.ToggleLoading(true);
        _isStartAnalysisButtonDisabled = true;

        try
        {
            await LoadCodebaseAsync(eventArgs.File);
            SetNamespaceComponents();
            AutoMapNamespaceComponents();
            _uploadedFile = eventArgs.File;
        }
        catch (Exception e)
        {
            CodebaseService.Dispose();
            _folderPath = string.Empty;
            _resultMessage = e.Message;
        }

        _loadingIndicator?.ToggleLoading(false);
        _isStartAnalysisButtonDisabled = false;
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
            _unmappedNamespaceComponents.Add(new NamespaceViewModel
            {
                Id = id,
                Name = folderNames[id]
            });
        }
    }

    private void AutoMapNamespaceComponents()
    {
        foreach (var namespaceComponent in _unmappedNamespaceComponents)
        {
            var autoMappingComponent = _selectedArchitectureViewModel?.Components
                .FirstOrDefault(component =>
                    namespaceComponent.Name.ToLower().Equals(component.Name.ToLower()) ||
                    namespaceComponent.Name.ToLower().StartsWith($"{component.Name.ToLower()}.") ||
                    namespaceComponent.Name.ToLower().EndsWith($".{component.Name.ToLower()}") ||
                    namespaceComponent.Name.ToLower().Contains($".{component.Name.ToLower()}."));
            
            if (autoMappingComponent != null)
            {
                autoMappingComponent.NamespaceComponents.Add(namespaceComponent);
            }
        }
        
        _unmappedNamespaceComponents.RemoveAll(namespaceComponent => _selectedArchitectureViewModel?.Components
            .SelectMany(component => component.NamespaceComponents).Contains(namespaceComponent) ?? false);
    }
}
