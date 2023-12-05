using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Results.Charts;

public partial class BarChart : ComponentBase
{
    [Parameter, EditorRequired]
    public string[] Labels { get; set; } = Array.Empty<string>();

    [Parameter, EditorRequired]
    public int[] Data { get; set; } = Array.Empty<int>();

    [Parameter]
    public bool ShowLegend { get; set; }

    [Parameter]
    public bool ShowDataLabels { get; set; } = true;

    private ChartData[] _data = Array.Empty<ChartData>();

    protected override void OnInitialized()
    {
        if (Labels.Length != Data.Length)
        {
            throw new ArgumentException("Length of data vs. labels do not match");
        }

        _data = new ChartData[Data.Length];

        for (int i = 0; i < Data.Length; i++)
        {
            _data[i] = new()
            {
                Label = Labels[i],
                Value = Data[i]
            };
        }
    }
}
