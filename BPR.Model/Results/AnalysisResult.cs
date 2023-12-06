﻿using BPR.Model.Architectures;
using BPR.Model.Enums;

namespace BPR.Model.Results;

public class AnalysisResult
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime ResultStart { get; set; }
    public DateTime ResultEnd { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public ArchitecturalModel? ArchitecturalModel { get; set; }
    public List<ViolationType> ViolationTypes { get; set; } = new();
    public List<Violation> Violations { get; set; } = new();
    public ExtendedAnalysisResults? ExtendedAnalysisResults { get; set; }

    public AnalysisResult(string title)
    {
        Title = title;
    }
}