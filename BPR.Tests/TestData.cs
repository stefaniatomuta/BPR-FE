using BPR.Analysis.Models;

namespace BPR.Tests;

internal class TestData
{
    internal static List<UsingDirective> GenerateDummyUsingDirectives()
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

    internal static List<UsingDirective> GenerateDummyUsingDirectivesForNestedDependencies()
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

    internal static List<UsingDirective> GenerateDummySelfReferencingUsingDirectives()
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

    internal static AnalysisArchitecturalComponent GenerateDummyComponent(bool isTransitiveDependencyLayerOpen = false)
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
                            },
                            IsOpen = isTransitiveDependencyLayerOpen
                        }
                    }
                }
            }
        };
    }

    internal static AnalysisArchitecturalComponent GenerateDummyComponentWithNoDependencies()
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
