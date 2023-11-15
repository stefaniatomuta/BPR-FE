using BPR.Analysis.Enums;
using BPR.Analysis.Models;
using BPR.Analysis.Services;

namespace BPR.Tests;

[TestFixture]
public class AnalysisServiceTests
{
    [Test]
    public void GetNamespaceAnalysis_WithCorrectNamespace_ReturnsNoViolation()
    {
        //Arrange
        var list = new List<NamespaceDirective>() {
            new () {
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
        var list = new List<NamespaceDirective>() {
            new () {
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
        var usingList = GenerateDummyUsingDirectives();
        var component = GenerateDummyComponentWithNoDependencies();

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, component, false);

        //Assert
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithUsingStatementInDependencies_ReturnsNoViolation()
    {
        //Arrange
        var usingList = GenerateDummyUsingDirectives();
        var component = GenerateDummyComponent();

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, component, false);

        //Assert
        Assert.That(result, Is.Empty);

    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithUsingStatementNotInDependencies_ReturnsViolation()
    {
        //Arrange
        var usingList = GenerateDummyUsingDirectivesForNestedDependencies();
        var component = GenerateDummyComponent();

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, component, false);

        //Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result[0].Severity, Is.EqualTo(ViolationSeverity.Major));
        Assert.That(result[0].Type, Is.EqualTo(ViolationType.ForbiddenDependency));
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithNestedDependencyAndClosedArchitecture_ReturnsViolations()
    {
        // Arrange
        var usingDirectives = GenerateDummyUsingDirectivesForNestedDependencies();
        var component = GenerateDummyComponent();

        // Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingDirectives, component, false);

        // Assert
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithNestedDependencyAndOpenArchitecture_ReturnsNoViolations()
    {
        // Arrange
        var usingDirectives = GenerateDummyUsingDirectivesForNestedDependencies();
        var component = GenerateDummyComponent();

        // Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingDirectives, component, true);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetDependencyAnalysisOnComponent_WithSelfReferencingUsingStatement_ReturnsNoViolations()
    {
        // Arrange
        var usingDirectives = GenerateDummySelfReferencingUsingDirectives();
        var component = GenerateDummyComponent();

        // Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingDirectives, component, false);

        // Assert
        Assert.That(result, Is.Empty);
    }

    private static List<UsingDirective> GenerateDummyUsingDirectives()
    {
        return new List<UsingDirective>
        {
            new()
            {
                Using = "using BPR.Mediator",
                ComponentName = "BPRBlazor"
            }
        };
    }

    private static List<UsingDirective> GenerateDummyUsingDirectivesForNestedDependencies()
    {
        return new List<UsingDirective>
        {
            new()
            {
                Using = "using BPR.Persistence",
                ComponentName = "BPRBlazor"
            }
        };
    }

    private static List<UsingDirective> GenerateDummySelfReferencingUsingDirectives()
    {
        return new List<UsingDirective>
        {
            new()
            {
                Using = "using BPRBlazor",
                ComponentName = "BPRBlazor"
            }
        };
    }

    private static AnalysisArchitecturalComponent GenerateDummyComponent()
    {
        return new AnalysisArchitecturalComponent
        {
            Name = "1",
            NamespaceComponents = new()
            {
                new()
                {
                    Name = "BPRBlazor"
                }
            },
            Dependencies = new()
            {
                new()
                {
                    Name = "2",
                    NamespaceComponents = new()
                    {
                        new()
                        {
                            Name = "BPR.Mediator"
                        }
                    },
                    Dependencies = new()
                    {
                        new()
                        {
                            Name = "3",
                            NamespaceComponents = new()
                            {
                                new()
                                {
                                    Name = "BPR.Persistence"
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private static AnalysisArchitecturalComponent GenerateDummyComponentWithNoDependencies()
    {
        return new AnalysisArchitecturalComponent
        {
            Name = "1",
            NamespaceComponents = new()
            {
                new()
                {
                    Name = "BPRBlazor"
                }
            },
            Dependencies = new()
            {
            }
        };
    }
}