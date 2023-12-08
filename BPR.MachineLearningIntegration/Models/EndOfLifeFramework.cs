using System.Text.Json.Serialization;

namespace BPR.MachineLearningIntegration.Models;

public class EndOfLifeFramework
{
    [JsonPropertyName("version")]
    public string Version { get; init; } = string.Empty;

    [JsonPropertyName("status")]
    public bool Status { get; init; }
}