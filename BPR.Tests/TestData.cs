using BPR.Analysis.Models;
using BPR.Model.Architectures;

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

    internal static ArchitecturalComponent GenerateDummyComponent(bool isTransitiveDependencyLayerOpen = false)
    {
        return new ArchitecturalComponent()
        {
            Id = 1,
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPRBlazor"
                }
            },
            Dependencies = new List<ArchitecturalDependency>()
            {
                new()
                {
                    Id = 2,
                },
                new()
                {
                    Id = 3,
                    IsOpen = isTransitiveDependencyLayerOpen
                }
            }
        };
    }

    internal static ArchitecturalModel GenerateDummyModel(bool isTransitiveDependencyLayerOpen = false)
    {
        return new ArchitecturalModel()
        {
            Components = new List<ArchitecturalComponent>()
            {
                new()
                {
                    Name = "1",
                    Id = 1,
                    NamespaceComponents = new List<NamespaceModel>()
                    {
                        new()
                        {
                            Name = "BPRBlazor"
                        }
                    },
                    Dependencies = new List<ArchitecturalDependency>()
                    {
                        new()
                        {
                            Id = 2,
                        }
                    }
                },
                new()
                {
                    Name = "2",
                    Id = 2,
                    NamespaceComponents = new List<NamespaceModel>()
                    {
                        new()
                        {
                            Name = "BPR.Mediator"
                        }
                    },
                    Dependencies = new List<ArchitecturalDependency>()
                    {
                        new()
                        {
                            Id = 3,
                            IsOpen = isTransitiveDependencyLayerOpen
                        }
                    }
                },
                new()
                {
                    Name = "3",
                    Id = 3,
                    NamespaceComponents = new List<NamespaceModel>()
                    {
                        new()
                        {
                            Name = "BPR.Persistence"
                        }
                    }
                }
            }
        };
    }

    internal static ArchitecturalComponent GenerateDummyComponentWithNoDependencies()
    {
        return new ArchitecturalComponent
        {
            Name = "1",
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPRBlazor"
                }
            },
            Dependencies = new List<ArchitecturalDependency>()
        };
    }
}