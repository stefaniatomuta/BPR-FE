using BPR.Persistence.Models;
using BPRBE.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using MongoDB.Bson;

namespace BPRBE.Tests;

[TestFixture]
public class ValidatorTests
{
    private IValidator<ArchitecturalModelCollection> _architecturalModelValidator;
    private IValidator<RuleCollection> _ruleValidator;

    [SetUp]
    public void Setup()
    {
        _architecturalModelValidator = new ArchitecturalModelValidator();
        _ruleValidator = new RuleValidator();
    }

    [Test]
    public void Architectural_Model_Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var model = new ArchitecturalModelCollection { Name = "", Components = new List<ArchitecturalComponentCollection>() };

        // Act
        var result = _architecturalModelValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Model name is required");
    }

    [Test]
    public void Architectural_Model_Should_Have_Error_When_Components_Is_Empty()
    {
        // Arrange
        var model = new ArchitecturalModelCollection { Name = "ModelName", Components = new List<ArchitecturalComponentCollection>() };

        // Act
        var result = _architecturalModelValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Components)
            .WithErrorMessage("Model cannot have no components");
    }

    [Test]
    public void Architectural_Model_Should_Have_Error_When_Duplicate_Component_Names()
    {
        // Arrange
        var model = new ArchitecturalModelCollection
        {
            Name = "ModelName",
            Components = new List<ArchitecturalComponentCollection>
            {
                new() { Name = "Component1" },
                new() { Name = "Component2" },
                new() { Name = "Component1" }
            }
        };

        // Act
        var result = _architecturalModelValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Components)
            .WithErrorMessage("Duplicate names of components are not allowed");
    }

    [Test]
    public void Architectural_Model_Should_Not_Have_Error_When_Valid_Model()
    {
        // Arrange
        var model = new ArchitecturalModelCollection
        {
            Name = "ModelName",
            Components = new List<ArchitecturalComponentCollection>
            {
                new() { Name = "Component1" },
                new() { Name = "Component2" }
            }
        };

        // Act
        var result = _architecturalModelValidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Rule_Does_Not_Have_Name()
    {
        // Arrange
        var newRule = new RuleCollection()
        {
            Id = new ObjectId(),
            Description = "Test"
        };
        // Act
        var result = _ruleValidator.TestValidate(newRule);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("RuleCollection must contain a name");
    }
}