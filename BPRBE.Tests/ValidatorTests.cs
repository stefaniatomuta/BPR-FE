using BPR.Models.Persistence;
using BPRBE.Models.Persistence;
using BPRBE.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace BPRBE.Tests;

[TestFixture]
public class ValidatorTests
{
    private IValidator<MongoArchitecturalModel> _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new ArchitecturalModelValidator();
    }

    [Test]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var model = new MongoArchitecturalModel { Name = "", Components = new List<MongoArchitecturalComponent>() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Model name is required");
    }

    [Test]
    public void Should_Have_Error_When_Components_Is_Empty()
    {
        // Arrange
        var model = new MongoArchitecturalModel { Name = "ModelName", Components = new List<MongoArchitecturalComponent>() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Components)
            .WithErrorMessage("Model cannot have no components");
    }
    
    [Test]
    public void Should_Have_Error_When_Duplicate_Component_Names()
    {
        // Arrange
        var model = new MongoArchitecturalModel
        {
            Name = "ModelName",
            Components = new List<MongoArchitecturalComponent>
            {
                new() { Name = "Component1" },
                new() { Name = "Component2" },
                new() { Name = "Component1" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Components)
            .WithErrorMessage("Duplicate names of components are not allowed");
    }

    [Test]
    public void Should_Not_Have_Error_When_Valid_Model()
    {
        // Arrange
        var model = new MongoArchitecturalModel
        {
            Name = "ModelName",
            Components = new List<MongoArchitecturalComponent>
            {
                new() { Name = "Component1" },
                new() { Name = "Component2" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}