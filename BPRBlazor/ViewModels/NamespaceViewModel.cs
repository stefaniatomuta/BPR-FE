namespace BPRBlazor.ViewModels;

public class NamespaceViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public NamespaceViewModel(int id, string name)
    {
        Id = id;
        Name = name;
    }
}