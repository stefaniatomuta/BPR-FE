namespace BPR.Persistence.Config;

public class DatabaseConfig
{
    public static string Section { get; set; } = "DatabaseConfig";
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
    public string ArchitectureModelsCollectionName { get; set; } = default!;
    public string RulesCollectionName { get; set; } = default!;
    public string ResultsCollectionName { get; set; } = default!;
}