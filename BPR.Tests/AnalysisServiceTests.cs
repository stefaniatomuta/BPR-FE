using BPR.Analysis.Models;
using BPR.Analysis.Services;
using BPR.Model.Enums;

namespace BPR.Tests;

[TestFixture]
public class AnalysisServiceTests
{
    [Test]
    public void GetNamespaceAnalysis_WithCorrectNamespace_ReturnsNoViolation()
    {
        //Arrange
        var list = new List<NamespaceDirective>()
        {
            new()
            {
                Namespace = "namespace AnalysisTest.Tests",
                File = "AnalysisTestsBySombrero.cs",
                FilePath = "BPR/AnalysisTest/Tests/AnalysisTestsBySombrero.cs"
            }
        };
        var folderPath = "BPR";

        //Act
        var result = AnalysisService.GetNamespaceAnalysis(list, folderPath);

        //Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetNamespaceAnalysis_WithIncorrectNamespace_ReturnsViolation()
    {
        //Arrange
        var list = new List<NamespaceDirective>()
        {
            new()
            {
                Namespace = "namespace AnalysisTest.Testy",
                File = "AnalysisTestsBySombrero.cs",
                FilePath = "BPR/AnalysisTest/Tests/AnalysisTestsBySombrero.cs"
            }
        };
        var folderPath = "BPR";

        //Act
        var result = AnalysisService.GetNamespaceAnalysis(list, folderPath);

        //Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result[0].Severity, Is.EqualTo(ViolationSeverity.Minor));
        Assert.That(result[0].Type, Is.EqualTo(ViolationType.MismatchedNamespace));
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithUsingStatementButNoDependencies_ReturnsViolation()
    {
        //Arrange
        var usingList = TestData.GenerateDummyUsingDirectives();
        var model = TestData.GenerateDummyModel();
        var component = TestData.GenerateDummyComponentWithNoDependencies();
        
        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, model, component);

        //Assert
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithUsingStatementInDependencies_ReturnsNoViolation()
    {
        //Arrange
        var usingList = TestData.GenerateDummyUsingDirectives();
        var model = TestData.GenerateDummyModel();
        var component = TestData.GenerateDummyComponent();
        
        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, model, component);

        //Assert
        Assert.That(result, Is.Empty);

    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithUsingStatementNotInDependencies_ReturnsViolation()
    {
        //Arrange
        var usingList = TestData.GenerateDummyUsingDirectivesForNestedDependencies();
        var model = TestData.GenerateDummyModel();
        var component = model.Components.First(comp => comp.Id == 1);

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, model, component);

        //Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result[0].Severity, Is.EqualTo(ViolationSeverity.Major));
        Assert.That(result[0].Type, Is.EqualTo(ViolationType.ForbiddenDependency));
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithNestedDependencyAndClosedArchitecture_ReturnsViolations()
    {
        // Arrange
        var usingDirectives = TestData.GenerateDummyUsingDirectivesForNestedDependencies();
        var model = TestData.GenerateDummyModel();
        var component = model.Components.First(comp => comp.Id == 1);

        // Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingDirectives, model, component);

        // Assert
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithNestedDependencyAndOpenArchitecture_ReturnsNoViolations()
    {
        // Arrange
        var usingDirectives = TestData.GenerateDummyUsingDirectivesForNestedDependencies();
        var component = TestData.GenerateDummyComponent(true);
        var model = TestData.GenerateDummyModel(true);

        // Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingDirectives, model, component);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithSelfReferencingUsingStatement_ReturnsNoViolations()
    {
        // Arrange
        var usingDirectives = TestData.GenerateDummySelfReferencingUsingDirectives();
        var model = TestData.GenerateDummyModel();
        var component = model.Components.First(comp => comp.Id == 1);

        // Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingDirectives, model, component);

        // Assert
        Assert.That(result, Is.Empty);
    }
}