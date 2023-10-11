﻿using BPRBE.Models.Persistence;

namespace BPRBE.Models;

public class ArchitecturalComponent
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitecturalComponent> Dependencies { get; set; }
    public Position Position { get; set; }
}