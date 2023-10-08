using BPRBE.Models.Persistence;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SevenZipExtractor;

namespace BPRBlazor.Pages;

public partial class Index : ComponentBase
{
    private string _errorMessage = string.Empty;
    private string _folderPath = string.Empty;
    private ArchitecturalModelViewModel _architecturalModelViewModel = default!;
    private List<NamespaceViewModel> _unmappedNamespaceComponents = new();
    private NamespaceViewModel _selectedNamespaceViewModelComponent = default!;
    private ArchitecturalModel _selectedArchitectureModel = default!;
    public List<RuleViewModel> _rulesViewModel = new();
    
    private async Task SendDataAsync()
    {
        await HttpService.PostAsync("http://127.0.0.1:8000/post?item=HelloWorld", string.Empty);
    }

    protected override void OnInitialized()
    {
        _errorMessage = default!;
        _architecturalModelViewModel = default!;
        LoadDummyData();
    }
    private void HandleArchitecturalModelOnChange(ArchitecturalModel architecturalModel)
    {
        _selectedArchitectureModel = architecturalModel;
    }

    private void HandleRule(RuleViewModel value)
    {
        var index  = _rulesViewModel.FindIndex(x => x.Name.Equals(value.Name));
        if (index != -1)
        {
            _rulesViewModel[index] = value;
        }
        else
        {
            _rulesViewModel.Add(value);
        }
    }

    private async Task StartAnalysis()
    {
        if (_selectedArchitectureModel == null)
        {
            _errorMessage = "Analysis cannot start without a selected architectural model";
        }
        if (_unmappedNamespaceComponents.Any())
        {
            _errorMessage = "Analysis can not start while there are unmapped namespaces";
            return;
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
    }

    private void LoadDummyData()
    {
        _architecturalModelViewModel = new ArchitecturalModelViewModel()
        {
            Name = "Architecture",
            Components = new List<ArchitecturalComponentViewModel>()
            {
                new ArchitecturalComponentViewModel()
                {
                    Id = 0,
                    Name = "Component0",
                    NamespaceComponents = new List<NamespaceViewModel>()
                },
                new ArchitecturalComponentViewModel()
                {
                    Id = 1,
                    Name = "Component1",
                    NamespaceComponents = new List<NamespaceViewModel>()
                },
                new ArchitecturalComponentViewModel()
                {
                    Id = 2,
                    Name = "Component2",
                    NamespaceComponents = new List<NamespaceViewModel>()
                },
                new ArchitecturalComponentViewModel()
                {
                    Id = 3,
                    Name = "Component3",
                    NamespaceComponents = new List<NamespaceViewModel>()
                },
            }
        };
    }
}