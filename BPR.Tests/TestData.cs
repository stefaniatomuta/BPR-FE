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
                Using = "using BPR.Mediator;",
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
                Using = "using BPR.Persistence;",
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
                Using = "using BPRBlazor;",
                ComponentName = "BPRBlazor"
            }
        };
    }

    internal static List<UsingDirective> GenerateOnionUsings()
    {
        return new List<UsingDirective>
        {
            new()
            {
                Using = "using BPR.Model;",
                ComponentName = "BPR.Mediator"
            },
            new()
            {
                Using = "using BPR.Model;",
                ComponentName = "BPRBlazor"
            },
            new()
            {
                Using = "using BPR.Model;",
                ComponentName = "BPR.Persistence"
            },
            new()
            {
                Using = "using BPR.Model;",
                ComponentName = "BPR.Analysis"
            },
            new()
            {
                Using = "using BPR.Mediator;",
                ComponentName = "BPRBlazor"
            },
            new()
            {
                Using = "using BPR.Mediator;",
                ComponentName = "BPR.Persistence"
            },
            new()
            {
                Using = "using BPR.Mediator;",
                ComponentName = "BPR.Analysis"
            }
        };
    }

    internal static ArchitectureComponent GenerateDummyComponent(bool isTransitiveDependencyLayerOpen = false)
    {
        return new ArchitectureComponent()
        {
            Id = 1,
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPRBlazor"
                }
            },
            Dependencies = new List<ArchitectureDependency>()
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

    internal static ArchitectureModel GenerateDummyModel(bool isTransitiveDependencyLayerOpen = false)
    {
        return new ArchitectureModel()
        {
            Components = new List<ArchitectureComponent>()
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
                    Dependencies = new List<ArchitectureDependency>()
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
                    Dependencies = new List<ArchitectureDependency>()
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

    internal static ArchitectureModel GenerateDummyModelWithDoubledMiddleLayer(bool isTransitiveDependencyLayerOpen = false)
    {
        return new ArchitectureModel()
        {
            Components = new List<ArchitectureComponent>()
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
                    Dependencies = new List<ArchitectureDependency>()
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
                            Name = "BPR.Mediator1"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>()
                    {
                        new()
                        {
                            Id = 4,
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
                            Name = "BPR.Mediator2"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>()
                    {
                        new()
                        {
                            Id = 4,
                            IsOpen = !isTransitiveDependencyLayerOpen
                        }
                    }
                },
                new()
                {
                    Name = "4",
                    Id = 4,
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

    internal static ArchitectureComponent GenerateDummyComponentWithNoDependencies()
    {
        return new ArchitectureComponent
        {
            Name = "1",
            NamespaceComponents = new List<NamespaceModel>()
            {
                new()
                {
                    Name = "BPRBlazor"
                }
            },
            Dependencies = new List<ArchitectureDependency>()
        };
    }

    internal static ArchitectureModel GenerateOnionModel(bool isApplicationLayerOpen)
    {
        return new ArchitectureModel
        {
            Components = new List<ArchitectureComponent>
            {
                new()
                {
                    Name = "Domain",
                    Id = 0,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "BPR.Model"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>()
                },
                new()
                {
                    Name = "Application",
                    Id = 1,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "BPR.Mediator"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 0,
                            IsOpen = isApplicationLayerOpen
                        }
                    }
                },
                new()
                {
                    Name = "Persistence",
                    Id = 2,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "BPR.Persistence"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 1,
                            IsOpen = false
                        }
                    }
                },
                new()
                {
                    Name = "Presentation",
                    Id = 3,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "BPRBlazor"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 1,
                            IsOpen = false
                        }
                    }
                },
                new()
                {
                    Name = "Infrastructure",
                    Id = 4,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "BPR.Analysis"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 1,
                            IsOpen = false
                        }
                    }
                },
            }
        };
    }

    internal static List<UsingDirective> GenerateComplexUsings()
    {
        return new List<UsingDirective>
        {
            new()
            {
                Using = "1Name;",
                ComponentName = "0Name"
            },
            new()
            {
                Using = "8Name;",
                ComponentName = "7Name"
            },
            new()
            {
                Using = "4Name;",
                ComponentName = "2Name"
            },
            new()
            {
                Using = "6Name;",
                ComponentName = "2Name"
            },
            new()
            {
                Using = "5Name;",
                ComponentName = "2Name"
            },
            new()
            {
                Using = "9Name;",
                ComponentName = "8Name"
            },
            new()
            {
                Using = "7Name;",
                ComponentName = "1Name"
            },
            new()
            {
                Using = "9Name;",
                ComponentName = "7Name"
            },

            // Violations
            new()
            {
                Using = "8Name;",
                ComponentName = "1Name"
            },
            new()
            {
                Using = "0Name;",
                ComponentName = "1Name"
            },
            new()
            {
                Using = "9Name;",
                ComponentName = "3Name"
            },
            new()
            {
                Using = "4Name;",
                ComponentName = "5Name"
            },
            new()
            {
                Using = "6Name;",
                ComponentName = "5Name"
            }
        };
    }

    internal static ArchitectureModel GenerateComplexModel()
    {
        return new ArchitectureModel
        {
            Components = new List<ArchitectureComponent>
            {
                new()
                {
                    Name = "0",
                    Id = 0,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "0Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 1,
                            IsOpen = true
                        }
                    }
                },
                new()
                {
                    Name = "1",
                    Id = 1,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "1Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 2,
                            IsOpen = true
                        },
                        new()
                        {
                            Id = 7,
                            IsOpen = true
                        }
                    }
                },
                new()
                {
                    Name = "2",
                    Id = 2,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "2Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 4,
                            IsOpen = true
                        },
                        new()
                        {
                            Id = 6,
                            IsOpen = true
                        }
                    }
                },
                new()
                {
                    Name = "3",
                    Id = 3,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "3Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 1,
                            IsOpen = true
                        }
                    }
                },
                new()
                {
                    Name = "4",
                    Id = 4,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "4Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 5,
                            IsOpen = true
                        }
                    }
                },
                new()
                {
                    Name = "5",
                    Id = 5,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "5Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>()
                },
                new()
                {
                    Name = "6",
                    Id = 6,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "6Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 5,
                            IsOpen = false
                        }
                    }
                },
                new()
                {
                    Name = "7",
                    Id = 7,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "7Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 8,
                            IsOpen = false
                        }
                    }
                },
                new()
                {
                    Name = "8",
                    Id = 8,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "8Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency>
                    {
                        new()
                        {
                            Id = 9,
                            IsOpen = true
                        }
                    }
                },
                new()
                {
                    Name = "9",
                    Id = 9,
                    NamespaceComponents = new List<NamespaceModel>
                    {
                        new()
                        {
                            Name = "9Name"
                        }
                    },
                    Dependencies = new List<ArchitectureDependency> ()
                }
            }
        };
    }
}