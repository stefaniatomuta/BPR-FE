using BPR.Persistence.Models;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using BPRBE.Models;
using BPRBE.Services;
using BPRBE.Validators;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace BPRBE.Tests;

[TestFixture]
public class RuleServiceTests
{
    private IRuleService uut;
    private IRuleRepository _repositoryStub;
    private IValidatorService _validatorService;
    private ILogger<RuleService> _logger;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _repositoryStub = Substitute.For<IRuleRepository>();
        _validatorService = Substitute.For<IValidatorService>();
        _logger = Substitute.For<ILogger<RuleService>>();
        uut = new RuleService(_repositoryStub, _validatorService, _logger);
    }

    [Test]
    public async Task GetRules_WhenRulesExist_ReturnsList()
    {
        // Arrange
        var list = new List<RuleCollection>()
        {
            new RuleCollection()
            {
                Name = "Dependency",
                Id = new ObjectId(),
                Description = "Check dependencies between components"
            }
        };
        _repositoryStub.GetRulesAsync().Returns(list);

        // Act
        var result = await uut.GetRulesAsync();

        // Assert
        Assert.That(list, Is.EqualTo(result));
    }
    [Test]
    public async Task AddRules_WhenRuleWithSameNameAlreadyExists_ReturnsFalse()
    {
        // Arrange
        var newRule = new RuleCollection()
        {
            Name = "Dependency"
        };

        var ruleModel = new Rule()
        {
            Name = "Dependency"

        };

        _validatorService.ValidateRuleAsync(ruleModel).Returns(new Result(true));
        _repositoryStub.AddRuleAsync(newRule).Returns(new Result(false));

        // Act
        var result = await uut.AddRuleAsync(ruleModel);

        // Assert
        Assert.That(result.Success, Is.False);
    }

}