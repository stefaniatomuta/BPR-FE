using BPRBlazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SevenZipExtractor;

namespace BPRBlazor.Pages;

public partial class Index : ComponentBase
{
    private string _errorMessage = string.Empty;
    private string _folderPath = string.Empty;
    private ArchitecturalModel _architecturalModel = default!;
    private List<Namespace> _unmappedNamespaceComponents = new();


    private async Task SendDataAsync()
    {
        await HttpService.PostAsync("http://127.0.0.1:8000/post?item=HelloWorld", string.Empty);
    }

    protected override void OnInitialized()
    {
        LoadDummyData();
    }

    private async Task LoadCodeSource(InputFileChangeEventArgs eventArgs)
    {
        _errorMessage = string.Empty;
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
        for (var id = 0; id < folderNames.Count; id++)
        {
            _unmappedNamespaceComponents.Add(new Namespace(id, folderNames[id]));
        }
    }

    public void Dispose() {
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
                new ArchitecturalComponent()
                {
                    Id = 4,
                    Name = "Component4",
                    NamespaceComponents = new List<Namespace>()
                },
                new ArchitecturalComponent()
                {
                    Id = 5,
                    Name = "Component5",
                    NamespaceComponents = new List<Namespace>()
                },
                new ArchitecturalComponent()
                {
                    Id = 6,
                    Name = "Component6",
                    NamespaceComponents = new List<Namespace>()
                },
                new ArchitecturalComponent()
                {
                    Id = 7,
                    Name = "Component7",
                    NamespaceComponents = new List<Namespace>()
                },
                new ArchitecturalComponent()
                {
                    Id = 8,
                    Name = "Component8",
                    NamespaceComponents = new List<Namespace>()
                },
            }
        };
    }
}