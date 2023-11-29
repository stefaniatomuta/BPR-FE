using System.Text.Json.Serialization;

namespace BPR.Model.Requests;

public class MLAnalysisRequestModel
{
    [JsonPropertyName("path")]
    public string Path { get; private set; }

    [JsonPropertyName("rules")]
    public List<string> Rules { get; private set; }

    public MLAnalysisRequestModel(string path, List<string> rules)
    {
        Path = path.Replace("\\", "/");
        Rules = rules;
    }
}