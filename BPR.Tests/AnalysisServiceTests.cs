using BPR.Analysis.Models;
using BPR.Analysis.Services.Analyses;
using BPR.Model.Architectures;
using BPR.Model.Enums;
using BPR.Model.Results;

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
        var result = NamespaceAnalysis.GetNamespaceAnalysis(list, folderPath);

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
        var result = NamespaceAnalysis.GetNamespaceAnalysis(list, folderPath);

        //Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result[0].Severity, Is.EqualTo(ViolationSeverity.Minor));
        Assert.That(result[0].Type, Is.EqualTo(ViolationType.MismatchedNamespace));
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithUsingStatementButNoDependencies_ReturnsViolation()
    {
        //Arrange
        var usings = TestData.GenerateDummyUsingDirectives();
        var model = TestData.GenerateDummyModel();

        //Act
        var violations = StartAnalysis(usings, model);

        //Assert
        Assert.That(violations, Is.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithUsingStatementInDependencies_ReturnsNoViolation()
    {
        //Arrange
        var usings = TestData.GenerateDummyUsingDirectives();
        var model = TestData.GenerateDummyModel();

        //Act
        var violations = StartAnalysis(usings, model);

        // Assert
        Assert.That(violations, Is.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithUsingStatementNotInDependencies_ReturnsViolation()
    {
        //Arrange
        var usings = TestData.GenerateDummyUsingDirectivesForNestedDependencies();
        var model = TestData.GenerateDummyModel();

        //Act
        var violations = StartAnalysis(usings, model);

        //Assert
        Assert.That(violations, Is.Not.Empty);
        Assert.That(violations[0].Severity, Is.EqualTo(ViolationSeverity.Major));
        Assert.That(violations[0].Type, Is.EqualTo(ViolationType.ForbiddenDependency));
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithNestedDependencyAndClosedArchitecture_ReturnsViolations()
    {
        // Arrange
        var usings = TestData.GenerateDummyUsingDirectivesForNestedDependencies();
        var model = TestData.GenerateDummyModel();

        // Act
        var violations = StartAnalysis(usings, model);

        // Assert
        Assert.That(violations, Is.Not.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithNestedDependencyAndOpenArchitecture_ReturnsNoViolations()
    {
        // Arrange
        var usings = TestData.GenerateDummyUsingDirectivesForNestedDependencies();
        var model = TestData.GenerateDummyModel(true);

        // Act
        var violations = StartAnalysis(usings, model);

        // Assert
        Assert.That(violations, Is.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithSelfReferencingUsingStatement_ReturnsNoViolations()
    {
        // Arrange
        var usings = TestData.GenerateDummySelfReferencingUsingDirectives();
        var model = TestData.GenerateDummyModel();

        // Act
        var violations = StartAnalysis(usings, model);

        // Assert
        Assert.That(violations, Is.Empty);
    }

    [TestCase(true, 0)]
    [TestCase(false, 3)]
    public void DependencyAnalysis_OnionModel_GivesAppropriate(bool isApplicationLayerOpen, int expectedViolations)
    {
        // Arrange
        var usings = TestData.GenerateOnionUsings();
        var model = TestData.GenerateOnionModel(isApplicationLayerOpen);

        // Act
        var violations = StartAnalysis(usings, model);

        // Assert
        Assert.That(violations, Has.Count.EqualTo(expectedViolations));
    }

    [Test]
    public void DependencyAnalysis_ComplexModel_GiveExceptedViolations()
    {
        // Arrange
        var usings = TestData.GenerateComplexUsings();
        var model = TestData.GenerateComplexModel();

        // Act
        var violations = StartAnalysis(usings, model);
        
        // Assert
        Assert.That(violations, Has.Count.EqualTo(5));
    }

    private static List<Violation> StartAnalysis(List<UsingDirective> usings, ArchitectureModel model)
    {
        var violations = new List<Violation>();

        foreach (var component in model.Components)
        {
            var componentUsings = usings.Where(u => !component.NamespaceComponents.Any(n => u.Using.Contains(n.Name)))
            .Where(u => component.NamespaceComponents.Any(n => n.Name == u.ComponentName))
            .ToList();
            violations.AddRange(DependencyAnalysis.GetDependencyAnalysisOnComponent(componentUsings, model, component));
        }

        return violations;
    }
}