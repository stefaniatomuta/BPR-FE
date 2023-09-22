namespace BPRBE.Config;

public class DatabaseConfig
{
    public static string Section { get; set; } = "DatabaseConfig";
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
    public string DependenciesRuleCollectionName { get; set; } = default!;
}