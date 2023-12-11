using BPR.Mediator.Validators;
using BPR.Model.Architectures;
using BPR.Model.Rules;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BPR.Tests;

[TestFixture]
public class ValidatorTests
{
    private IValidator<ArchitectureModel> _architectureModelValidator = default!;
    private IValidator<Rule> _ruleValidator = default!;

    [SetUp]
    public void Setup()
    {
        _architectureModelValidator = new ArchitectureModelValidator();
        _ruleValidator = new RuleValidator();
    }

    [Test]
    public void Architecture_Model_Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var model = new ArchitectureModel {Name = "", Components = new List<ArchitectureComponent>()};

        // Act
        var result = _architectureModelValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Model name is required");
    }

    [Test]
    public void Architecture_Model_Should_Have_Error_When_Components_Is_Empty()
    {
        // Arrange
        var model = new ArchitectureModel {Name = "ModelName", Components = new List<ArchitectureComponent>()};

        // Act
        var result = _architectureModelValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Components)
            .WithErrorMessage("Model cannot have no components");
    }

    [Test]
    public void Architecture_Model_Should_Have_Error_When_Duplicate_Component_Names()
    {
        // Arrange
        var model = new ArchitectureModel
        {
            Name = "ModelName",
            Components = new List<ArchitectureComponent>
            {
                new() {Name = "Component1"},
                new() {Name = "Component2"},
                new() {Name = "Component1"}
            }
        };

        // Act
        var result = _architectureModelValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Components)
            .WithErrorMessage("Duplicate names of components are not allowed");
    }

    [Test]
    public void Architecture_Model_Should_Not_Have_Error_When_Valid_Model()
    {
        // Arrange
        var model = new ArchitectureModel
        {
            Name = "ModelName",
            Components = new List<ArchitectureComponent>
            {
                new() {Name = "Component1"},
                new() {Name = "Component2"}
            }
        };

        // Act
        var result = _architectureModelValidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Rule_Does_Not_Have_Name()
    {
        // Arrange
        var newRule = new Rule()
        {
            Id = new Guid(),
            Description = "Test"
        };
        // Act
        var result = _ruleValidator.TestValidate(newRule);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("RuleCollection must contain a name");
    }
}