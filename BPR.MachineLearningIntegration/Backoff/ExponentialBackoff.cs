using Microsoft.Extensions.Logging;

namespace BPR.MachineLearningIntegration.Backoff;

internal class ExponentialBackoff
{
    public TimeSpan Backoff { get; private set; }
    private readonly TimeSpan _maxBackoff;

    public ExponentialBackoff(TimeSpan initialBackoff, TimeSpan maxBackoff)
    {
        Backoff = initialBackoff;
        _maxBackoff = maxBackoff;
    }

    public async Task PerformBackoff(CancellationToken cancellationToken)
    {
        await Task.Delay(Backoff, cancellationToken);
        Backoff *= 2;

        if (Backoff > _maxBackoff)
        {
            Backoff = _maxBackoff;
        }
    }
}
