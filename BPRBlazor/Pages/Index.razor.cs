using BPRBlazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SevenZipExtractor;
using BE = BPRBE.Models.Persistence;

namespace BPRBlazor.Pages;

public partial class Index : ComponentBase
{
    private string _errorMessage = string.Empty;
    private string _folderPath = string.Empty;
    private ArchitecturalModel _architecturalModel = default!;
    private List<Namespace> _unmappedNamespaceComponents = new();
    private Namespace _selectedNamespaceComponent = default!;
    private BE.ArchitecturalModel? _selectedArchitectureModel;

    private async Task SendDataAsync()
    {
        await HttpService.PostAsync("http://127.0.0.1:8000/post?item=HelloWorld", string.Empty);
    }

    protected override void OnInitialized()
    {
        _errorMessage = default!;
        _architecturalModel = default!;
        LoadDummyData();
    }

    private void HandleArchitectureModelOnChange(BE.ArchitecturalModel newValue)
    {
        _selectedArchitectureModel = newValue;
        // TODO - Actually do something with the selected model when analysis is started.
    }

    private void StartAnalysis()
    {
        if (_unmappedNamespaceComponents.Any())
        {
            _errorMessage = "Analysis can not start while there are unmapped namespaces";
            return;
        }
    }

    private void HandleDrop(ArchitecturalComponent component = default!)
    {
        var oldComponent = _architecturalModel.Components.FirstOrDefault(architecturalComponent =>
            architecturalComponent.NamespaceComponents.Contains(_selectedNamespaceComponent)) ?? default!;
        if (oldComponent != default!)
        {
            oldComponent.NamespaceComponents.Remove(_selectedNamespaceComponent);
        }

        if (component != default!)
        {
            component.NamespaceComponents.Add(_selectedNamespaceComponent);
            _unmappedNamespaceComponents.Remove(_selectedNamespaceComponent);
        }
        else
        {
            _unmappedNamespaceComponents.Add(_selectedNamespaceComponent);
        }
    }

    private void HandleDragStart(Namespace namespaceComponent)
    {
        _selectedNamespaceComponent = namespaceComponent;
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
        _unmappedNamespaceComponents = new List<Namespace>();
        var folderNames = DependencyComponentService.GetFolderNamesForProjects(_folderPath);
        for (var id = 0; id < folderNames.Count(); id++)
        {
            _unmappedNamespaceComponents.Add(new Namespace(id, folderNames[id]));
        }
    }

    public void Dispose()
    {
        CodebaseService.Dispose();
    }

    private void LoadDummyData()
    {
        _architecturalModel = new ArchitecturalModel()
        {
            Name = "Architecture",
            Components = new List<ArchitecturalComponent>()
            {
                new ArchitecturalComponent()
                {
                    Id = 0,
                    Name = "Component0",
                    NamespaceComponents = new List<Namespace>()
                },
                new ArchitecturalComponent()
                {
                    Id = 1,
                    Name = "Component1",
                    NamespaceComponents = new List<Namespace>()
                },
                new ArchitecturalComponent()
                {
                    Id = 2,
                    Name = "Component2",
                    NamespaceComponents = new List<Namespace>()
                },
                new ArchitecturalComponent()
                {
                    Id = 3,
                    Name = "Component3",
                    NamespaceComponents = new List<Namespace>()
                },
            }
        };
    }
}