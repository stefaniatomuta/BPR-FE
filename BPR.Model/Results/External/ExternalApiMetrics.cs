namespace BPR.Model.Results.External;

public class ExternalApiMetrics
{
    public int Usage { get; set; }

    public string[] Versions { get; set; } = Array.Empty<string>();
}
