namespace BPR.Mediator.Utils;

public static class FolderCleanup
{
    public static void Cleanup(Guid guid)
    {
        Cleanup($"../temp/{guid}");
    }

    public static void Cleanup(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
}
