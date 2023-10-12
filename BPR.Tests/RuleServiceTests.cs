using AutoMapper;
using BPR.Persistence.Models;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using BPR.Mediator.Models;
using BPR.Mediator.Services;
using BPR.Mediator.Validators;
using Microsoft.Extensions.Logging;

namespace BPR.Tests;

[TestFixture]
public class RuleServiceTests
{
    private IRuleService uut;
    private IRuleRepository _repositoryStub;
    private IValidatorService _validatorService;
    private ILogger<RuleService> _logger;
    private IMapper _mapper;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _repositoryStub = Substitute.For<IRuleRepository>();
        _validatorService = Substitute.For<IValidatorService>();
        _logger = Substitute.For<ILogger<RuleService>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Rule, RuleCollection>();
            cfg.CreateMap<RuleCollection, Rule>();

        }); 
        _mapper = new Mapper(mapperConfig);
        uut = new RuleService(_repositoryStub, _validatorService,_mapper, _logger);
    }

    [Test]
    public async Task GetRules_WhenRulesExist_ReturnsList()
    {
        // Arrange
        var list = new List<RuleCollection>()
        {
            new ()
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
        _repositoryStub.AddRuleAsync(Arg.Any<RuleCollection>()).Returns(new Result(false));

        // Act
        var result = await uut.AddRuleAsync(rule);
        
        // Assert
        await _validatorService.Received(1).ValidateRuleAsync(Arg.Is(rule));
        await _repositoryStub.Received(1).AddRuleAsync(Arg.Any<RuleCollection>());
        Assert.That(result.Success, Is.False);
    }
}