using System.Text.Json.Serialization;

namespace BPR.MachineLearningIntegration.Models;

public class ExternalApiMetrics
{
    [JsonPropertyName("usage")]
    public int Usage { get; set; }

    [JsonPropertyName("versions")]
    public string[] Versions { get; set; } = Array.Empty<string>();
}
