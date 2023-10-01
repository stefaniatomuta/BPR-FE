namespace BPR.Models.Blazor;

public class Namespace
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Namespace(int id, string name)
    {
        Id = id;
        Name = name;
    }
}