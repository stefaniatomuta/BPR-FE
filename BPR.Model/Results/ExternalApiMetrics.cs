namespace BPR.Model.Results;

public class ExternalApiMetrics
{
    public int Usage { get; set; }

    public string[] Versions { get; set; } = Array.Empty<string>();
}
