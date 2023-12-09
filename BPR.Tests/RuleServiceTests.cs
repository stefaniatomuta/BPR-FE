using BPR.Mediator.Interfaces;
using BPR.Mediator.Services;
using BPR.Mediator.Utils;
using BPR.Mediator.Validators;
using BPR.Model.Architectures;
using Microsoft.Extensions.Logging;

namespace BPR.Tests;

[TestFixture]
public class RuleServiceTests
{
    private IRuleService uut;
    private IRuleRepository _repositoryStub;
    private IValidatorService _validatorService;
    private readonly ILogger<RuleService> _logger = default!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _repositoryStub = Substitute.For<IRuleRepository>();
        _validatorService = Substitute.For<IValidatorService>();
        uut = new RuleService(_repositoryStub, _validatorService, _logger);
    }

    [Test]
    public async Task GetRules_WhenRulesExist_ReturnsList()
    {
        // Arrange
        var list = new List<Rule>()
        {
            new()
            {
                Name = "Dependency",
                Id = new Guid(),
                Description = "Check dependencies between components"
            }
        };
        _repositoryStub.GetRulesAsync().Returns(list);

        // Act
        var result = await uut.GetRulesAsync();
        // Assert
        Assert.That(result, Has.Count.EqualTo(list.Count));
    }

    [Test]
    public async Task AddRules_WhenRuleWithSameNameAlreadyExists_ReturnsFalse()
    {
        // Arrange
        var rule = new Rule()
        {
            Id = Guid.NewGuid(),
            Name = "Dependency"
        };

        _validatorService.ValidateRuleAsync(rule).Returns(new Result(true));
        _repositoryStub.AddRuleAsync(Arg.Any<Rule>()).Returns(new Result(false));

        // Act
        var result = await uut.AddRuleAsync(rule);

        // Assert
        await _validatorService.Received(1).ValidateRuleAsync(Arg.Is(rule));
        await _repositoryStub.Received(1).AddRuleAsync(Arg.Any<Rule>());
        Assert.That(result.Success, Is.False);
    }
}