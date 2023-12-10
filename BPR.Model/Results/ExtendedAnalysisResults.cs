namespace BPR.Model.Results.External;

public class ExtendedAnalysisResults
{
    public Guid CorrelationId { get; set; }
    public List<Dictionary<string, EndOfLifeFramework>>? EndOfLifeFrameworks { get; init; }
    public int? IfFrequency { get; init; }
    public int? ForFrequency { get; init; }
    public int? ForEachFrequency { get; init; }
    public int? WhileFrequency { get; init; }
    public int? TotalCodeLines { get; init; }
    public List<Dictionary<string, int>>? CodeLinesPerFile { get; init; }
    public int? TotalCommentLines { get; init; }
    public List<Dictionary<string, int>>? CommentLinesPerFile { get; init; }
    public int? TotalClasses { get; init; }
    public int? TotalMethods { get; init; }
    public int? TotalInterfaces { get; init; }
    public int? TotalInheritanceDeclarations { get; init; }
    public int? TotalUsingDirectives { get; init; }
    public int? TotalCSharpFiles { get; init; }
    public Dictionary<string, ExternalApiMetrics>? ExternalApiCalls { get; init; }
    public int? TotalHttpClientCalls { get; init; }
    public Dictionary<string, int>? ClassCouplings { get; init; }
    public Dictionary<string, Dictionary<string, double>>? CodeSimilarities { get; init; }
    public string? TechnicalDebtClassification { get; init; }
}