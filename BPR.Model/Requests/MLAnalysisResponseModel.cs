using System.Text.Json.Serialization;

namespace BPR.Model.Requests;

public class MLAnalysisResponseModel
{
    [JsonPropertyName("endOfLifeFramework")]
    public List<Dictionary<string, EndOfLifeFramework>>? EndOfLifeFrameworks { get; set; }

    [JsonPropertyName("ifFrequency")]
    public int? IfFrequency { get; set; }

    [JsonPropertyName("forFrequency")]
    public int? ForFrequency { get; set; }

    [JsonPropertyName("forEachFrequency")]
    public int? ForEachFrequency { get; set; }

    [JsonPropertyName("whileFrequency")]
    public int? WhileFrequency { get; set; }

    [JsonPropertyName("codeLines")]
    public int? TotalCodeLines { get; set; }

    [JsonPropertyName("codeLinesPerFile")]
    public List<Dictionary<string, int>>? CodeLinesPerFile { get; set; }

    [JsonPropertyName("commentLines")]
    public int? TotalCommentLines { get; set; }

    [JsonPropertyName("commentLinesPerFile")]
    public List<Dictionary<string, int>>? CommentLinesPerFile { get; set; }

    [JsonPropertyName("classNumber")]
    public int? TotalClasses { get; set; }

    [JsonPropertyName("methodNumber")]
    public int? TotalMethods { get; set; }

    [JsonPropertyName("interfaceNumber")]
    public int? TotalInterfaces { get; set; }

    [JsonPropertyName("inheritanceDeclarations")]
    public int? TotalInheritanceDeclarations { get; set; }

    [JsonPropertyName("usingsNumber")]
    public int? TotalUsingDirectives { get; set; }

    [JsonPropertyName("externalApiCalls")]
    public Dictionary<string, int>? ExternalApiCalls { get; set; }

    [JsonPropertyName("httpClientCalls")]
    public int? TotalHttpClientCalls { get; set; }

    [JsonPropertyName("classCouplingListing")]
    public Dictionary<string, int>? ClassCouplings { get; set; }

    [JsonPropertyName("codeSimilarity")]
    public Dictionary<string, Dictionary<string, double>>? CodeSimilarities { get; set; }
}

public class EndOfLifeFramework
{
    public string Version { get; set; } = string.Empty;
    public bool Status { get; set; }
}
