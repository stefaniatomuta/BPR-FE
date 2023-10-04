using BPRBE.Models.Persistence;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class SelectRuleComponent : ComponentBase
{
    public IList<Rule> Rules = new List<Rule>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Rules = await GetRulesAsync();
    }
    
    private async Task<IList<Rule>> GetRulesAsync()
    {
        return await Service.GetRulesAsync();
    }
}