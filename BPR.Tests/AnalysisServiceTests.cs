using BPR.Analysis.Models;
using BPR.Analysis.Services;
using BPR.Model.Architectures;
using BPR.Model.Enums;

namespace BPR.Tests;

[TestFixture]
public class AnalysisServiceTests
{
    [Test]
    public void AnalyseNamespace_Returns_NoViolation()
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
    public void AnalyseNamespace_Returns_Violation()
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
        Assert.That(result, !Is.Empty);
        Assert.That(result[0].Severity, Is.EqualTo(ViolationSeverity.Minor));
        Assert.That(result[0].Type, Is.EqualTo(ViolationType.MismatchedNamespace));
    }

    [Test]
    public void AnalyseDependency_With1Component_Returns_NoViolation()
    {
        //Arrange
        var usingList = new List<UsingDirective>()
        {
            new()
            {
                Using = "using BPR.AnalysisTest.Tests",
                File = "DependencyTests.cs"
            }
        };
        var component = new ArchitecturalComponent()
        {
            Name = "Component X",
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPR/AnalysisTest"
                }
            },
            Dependencies = new List<ArchitecturalDependency>()
        };
        var model = new ArchitecturalModel()
        {
            Components = new List<ArchitecturalComponent>()
            {
                component
            }
        };

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, model, component);

        //Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void AnalyseDependency_WithComponents_Returns_NoViolation()
    {
        //Arrange
        var usingListDependency = new List<UsingDirective>()
        {
            new()
            {
                Using = "using BPR.Analysis.Models",
                File = "DependencyTests.cs"
            }
        };
       
        var componentX = new ArchitecturalComponent()
        {
            Name = "Component X",
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPR.Analysis.Services"
                }
            },
            Dependencies = new List<ArchitecturalDependency>()
            {
                new()
                {
                    Id = 1
                }
            }
        };
        var componentY = new ArchitecturalComponent()
        {
            Name = "Component Y",
            Id = 1,
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPR.Analysis.Models"
                }
            }
        };
        var model = new ArchitecturalModel()
        {
            Components = new List<ArchitecturalComponent>()
            {
                componentX,
                componentY
            },
        };

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingListDependency, model, componentX);

        //Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void AnalyseDependency_WithComponents_Returns_Violation()
    {
        //Arrange
        var usingList = new List<UsingDirective>()
        {
            new()
            {
                Using = "using BPR.Analysis.Models",
                File = "DependencyTests.cs"
            }
        };
        var usingListDependency = new List<UsingDirective>()
        {
            new()
            {
                Using = "using BPR.Persistence",
                File = "DependencyController.cs"
            }
        };
        var componentX = new ArchitecturalComponent()
        {
            Name = "Component X",
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPR/Analysis/Services"
                }
            },
            Dependencies = new List<ArchitecturalDependency>()
            {
                new()
                {
                    Id = 1,
                }
            }
        };
        var componentY = new ArchitecturalComponent()
        {
            Name = "Component Y",
            Id = 1,
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPR/Analysis/Models"
                }
            }
        };
        var model = new ArchitecturalModel()
        {
            Components = new List<ArchitecturalComponent>()
            {
                componentX,
                componentY
            },
        };

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, model, componentX);
        foreach (var dependency in model.Components.Where(comp => componentX.Dependencies.Select(dep => dep.Id).Contains(comp.Id)))
        {
            result.AddRange(AnalysisService.GetDependencyAnalysisOnComponent(usingListDependency, model, dependency));
        }

        //Assert
        Assert.That(result, !Is.Empty);
        Assert.That(result[0].Severity, Is.EqualTo(ViolationSeverity.Major));
        Assert.That(result[0].Type, Is.EqualTo(ViolationType.ForbiddenDependency));
    }
}