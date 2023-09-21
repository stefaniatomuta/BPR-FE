namespace BPRBE.Config;

public class DatabaseConfig
{
    public static string Section { get; set; } = "DatabaseConfig";
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string DependenciesRuleCollectionName { get; set; }
}