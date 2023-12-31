﻿using System.ComponentModel.DataAnnotations;

namespace BPRBlazor.ViewModels;

public class ArchitectureComponentViewModel
{
    public int Id { get; set; }

    public PositionViewModel Position { get; set; } = new();
    public SizeViewModel? Size { get; set; }

    [Required(ErrorMessage = "Please give the component a name")]
    public string Name { get; set; } = string.Empty;

    public List<DependencyViewModel> Dependencies { get; set; } = new();

    public List<NamespaceViewModel> NamespaceComponents { get; set; } = new();
}
