namespace BPRBlazor.Pages;

public partial class Index : ComponentBase
{
    private string _errorMessage = string.Empty;
    private List<string> _folders = new ();


    private async Task SendDataAsync()
    {
        await HttpService.PostAsync("http://127.0.0.1:8000/post?item=HelloWorld", string.Empty);
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
            await using var memoryStream = new MemoryStream();
            await eventArgs.File.OpenReadStream(Int32.MaxValue).CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            using var archiveFile = new ArchiveFile(memoryStream, SevenZipFormat.SevenZip);
            _folders = DependencyComponentService.LoadComponentsFromStream(archiveFile);
            // foreach (var entry in archiveFile.Entries)
            // {
            //     // Console.WriteLine(entry.FileName);
            // }
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " " + e.StackTrace;
        }
    }
}