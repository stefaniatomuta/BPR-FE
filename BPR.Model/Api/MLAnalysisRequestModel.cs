using System.Text.Json.Serialization;

namespace BPR.Model.Api;

public class MLAnalysisRequestModel
{
    [JsonPropertyName("correlation_id")]
    public Guid CorrelationId { get; init; }

    [JsonPropertyName("path")]
    public string Path { get; init; }

    [JsonPropertyName("rules")]
    public List<string> Rules { get; init; }

    public MLAnalysisRequestModel(string path, List<string> rules, Guid correlationId)
    {
        Path = path.Replace("\\", "/");
        Rules = rules;
        CorrelationId = correlationId;
    }
}