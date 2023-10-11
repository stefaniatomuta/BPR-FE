using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using BPRBE.Services;
using BPRBE.Validators;
using MongoDB.Bson;

namespace BPRBE.Tests;

[TestFixture]
public class RuleServiceTests
{
    private IRuleService uut;
    private IRuleRepository _repositoryStub;
    private IValidatorService _validatorService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _repositoryStub = Substitute.For<IRuleRepository>();
        _validatorService = Substitute.For<IValidatorService>();
        uut = new RuleService(_repositoryStub, _validatorService);
    }

    [Test]
    public async Task GetRules_WhenRulesExist_ReturnsList()
    {
        // Arrange
        var list = new List<Rule>()
        {
            new Rule()
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
        var newRule = new Rule()
        {
            Name = "Dependency"
        };

        _validatorService.ValidateRuleAsync(newRule).Returns(new Result(true));
        _repositoryStub.AddRuleAsync(newRule).Returns(new Result(false));

        // Act
        var result = await uut.AddRuleAsync(newRule);

        // Assert
        Assert.That(result.Success, Is.False);
    }

}