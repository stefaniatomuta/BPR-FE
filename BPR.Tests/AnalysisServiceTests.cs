using BPR.Analysis.Enums;
using BPR.Analysis.Models;
using BPR.Analysis.Services;

namespace BPR.Tests; 

[TestFixture]
public class AnalysisServiceTests {
    [Test]
    public void AnalyseNamespace_Returns_NoViolation() {
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
        var result = AnalysisService.GetNamespaceAnalysis(list,folderPath);
        
        //Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void AnalyseNamespace_Returns_Violation() {
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
        var result = AnalysisService.GetNamespaceAnalysis(list,folderPath);
        
        //Assert
        Assert.That(result, !Is.Empty);
        Assert.That(result[0].Severity,Is.EqualTo(ViolationSeverity.Minor));
        Assert.That(result[0].Type,Is.EqualTo(ViolationType.MismatchedNamespace));
    }

    [Test]
    public void AnalyseDependency_With1Component_Returns_NoViolation() {
        //Arrange
        var usingList = new List<UsingDirective>() {
            new () {
                Using = "using BPR.AnalysisTest.Tests",
                File = "DependencyTests.cs"
            }
        };
        var component = new AnalysisArchitecturalComponent() {
            Name = "Component X",
            NamespaceComponents = new List<AnalysisNamespace>() {
                new() {
                    Name = "BPR/AnalysisTest"
                }
            },
            Dependencies = new List<AnalysisArchitecturalComponent>()
        };

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, component);
        
        //Assert
        Assert.That(result,Is.Empty);
    }
    
    [Test]
    public void AnalyseDependency_WithComponents_Returns_NoViolation() {
        //Arrange
        var usingListDependency = new List<UsingDirective>() {
            new() {
                
                Using = "using BPR.Analysis.Models",
                File = "DependencyTests.cs"
            }
        };
        var component = new AnalysisArchitecturalComponent() {
            Name = "Component X",
            NamespaceComponents = new List<AnalysisNamespace>() {
                new() {
                    Name = "BPR.Analysis.Services"
                }
            },
            Dependencies = new List<AnalysisArchitecturalComponent>() {
                new () {
                    Name = "Component Y",
                    NamespaceComponents = new List<AnalysisNamespace>() {
                        new () {
                            Name = "BPR.Analysis.Models"
                        }
                    }
                    
                }
            }
        };

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingListDependency, component);
        
        //Assert
        Assert.That(result,Is.Empty);

    }
    
    [Test]
    public void AnalyseDependency_WithComponents_Returns_Violation() {
        //Arrange
        var usingList = new List<UsingDirective>() {
            new() {
                Using = "using BPR.Analysis.Models",
                File = "DependencyTests.cs"
            }
        };
        var usingListDependency = new List<UsingDirective>(){
            new () {
                Using = "using BPR.Persistence",
                File = "DependencyController.cs"
            }
        };
        var component = new AnalysisArchitecturalComponent() {
            Name = "Component X",
            NamespaceComponents = new List<AnalysisNamespace>() {
                new() {
                    Name = "BPR/Analysis/Services"
                }
            },
            Dependencies = new List<AnalysisArchitecturalComponent>() {
                new (){
                    Name = "Component Y",
                    NamespaceComponents = new List<AnalysisNamespace>() {
                        new () {
                            Name = "BPR/Analysis/Models"
                        }
                    }
                    
                }
            }
        };

        //Act
        var result = AnalysisService.GetDependencyAnalysisOnComponent(usingList, component);
        foreach (var dependency in component.Dependencies) {
            result.AddRange(AnalysisService.GetDependencyAnalysisOnComponent(usingListDependency,dependency));
        }
        //Assert
        Assert.That(result, !Is.Empty);
        Assert.That(result[0].Severity,Is.EqualTo(ViolationSeverity.Major));
        Assert.That(result[0].Type,Is.EqualTo(ViolationType.ForbiddenDependency));
    }
}