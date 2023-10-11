using BPR.Persistence.Models;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using BPRBE.Models;
using BPRBE.Validators;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace BPRBE.Services;

public class RuleService : IRuleService
{
    private readonly IRuleRepository _ruleRepository;
    private readonly IValidatorService _validatorService;

    public RuleService(IRuleRepository ruleRepository, IValidatorService validatorService, ILogger<RuleService> logger)
    {
        _ruleRepository = ruleRepository;
        _validatorService = validatorService;
    }

    // TODO - Is this method necessary? We have no user stories related to being able to add new rules.
    public async Task<Result> AddRuleAsync(Rule rule)
    {
        var result = await _validatorService.ValidateRuleAsync(rule);
        if (result.Success)
        {
            var document = new RuleCollection()
            {
                Id = ObjectId.Parse(rule.Id.ToString()),
                Name = rule.Name,
                Description = rule.Description
            };
            return await _ruleRepository.AddRuleAsync(document);
        }
        return result;
    }

    public async Task<IList<Rule>> GetRulesAsync()
    {
        var rules = await _ruleRepository.GetRulesAsync();
        var documents = new List<Rule>();
        foreach (var doc in rules)
        {
            var document = new Rule()
            {
                Id = Guid.Parse(doc.Id.ToString()),
                Name = doc.Name,
                Description = doc.Description
            };
            documents.Add(document);
        }
        return documents;
    }
}