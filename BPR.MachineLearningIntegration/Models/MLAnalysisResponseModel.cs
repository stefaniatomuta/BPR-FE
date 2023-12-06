using BPR.Model.Results;
using System.Text.Json.Serialization;

namespace BPR.MachineLearningIntegration.Models;

public class MLAnalysisResponseModel
{
    [JsonPropertyName("correlationId")]
    public Guid CorrelationId { get; set; }

    [JsonPropertyName("endOfLifeFramework")]
    public List<Dictionary<string, EndOfLifeFramework>>? EndOfLifeFrameworks { get; init; }

    [JsonPropertyName("ifFrequency")]
    public int? IfFrequency { get; init; }

    [JsonPropertyName("forFrequency")]
    public int? ForFrequency { get; init; }

    [JsonPropertyName("forEachFrequency")]
    public int? ForEachFrequency { get; init; }

    [JsonPropertyName("whileFrequency")]
    public int? WhileFrequency { get; init; }

    [JsonPropertyName("codeLines")]
    public int? TotalCodeLines { get; init; }

    [JsonPropertyName("codeLinesPerFile")]
    public List<Dictionary<string, int>>? CodeLinesPerFile { get; init; }

    [JsonPropertyName("commentLines")]
    public int? TotalCommentLines { get; init; }

    [JsonPropertyName("commentLinesPerFile")]
    public List<Dictionary<string, int>>? CommentLinesPerFile { get; init; }

    [JsonPropertyName("csFiles")]
    public int? TotalCSharpFiles { get; init; }

    [JsonPropertyName("classNumber")]
    public int? TotalClasses { get; init; }

    [JsonPropertyName("methodNumber")]
    public int? TotalMethods { get; init; }

    [JsonPropertyName("interfaceNumber")]
    public int? TotalInterfaces { get; init; }

    [JsonPropertyName("inheritanceDeclarations")]
    public int? TotalInheritanceDeclarations { get; init; }

    [JsonPropertyName("usingsNumber")]
    public int? TotalUsingDirectives { get; init; }

    [JsonPropertyName("externalApiCalls")]
    public Dictionary<string, int>? ExternalApiCalls { get; init; }

    [JsonPropertyName("httpClientCalls")]
    public int? TotalHttpClientCalls { get; init; }

    [JsonPropertyName("classCouplingListing")]
    public Dictionary<string, int>? ClassCouplings { get; init; }

    [JsonPropertyName("codeSimilarity")]
    public Dictionary<string, Dictionary<string, double>>? CodeSimilarities { get; init; }
}
