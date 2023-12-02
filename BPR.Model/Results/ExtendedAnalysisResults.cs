using BPR.Model.Enums;

namespace BPR.Model.Results;

public class ExtendedAnalysisResults
{
    public Guid CorrelationId { get; set; }
    public List<Dictionary<string, EndOfLifeFramework>>? EndOfLifeFrameworks { get; init; }
    public int? IfFrequency { get; init; }
    public int? ForFrequency { get; init; }
    public int? ForEachFrequency { get; init; }
    public int? WhileFrequency { get; init; }
    public int? TotalCodeLines { get; init; }
    public List<Dictionary<string, int>>? CodeLinesPerFile { get; init; }
    public int? TotalCommentLines { get; init; }
    public List<Dictionary<string, int>>? CommentLinesPerFile { get; init; }
    public int? TotalClasses { get; init; }
    public int? TotalMethods { get; init; }
    public int? TotalInterfaces { get; init; }
    public int? TotalInheritanceDeclarations { get; init; }
    public int? TotalUsingDirectives { get; init; }
    public Dictionary<string, int>? ExternalApiCalls { get; init; }
    public int? TotalHttpClientCalls { get; init; }
    public Dictionary<string, int>? ClassCouplings { get; init; }
    public Dictionary<string, Dictionary<string, double>>? CodeSimilarities { get; init; }

    public List<Violation> MapToViolations()
    {
        var violations = new List<Violation>();
        AddConditionalStatementsViolations(violations);
        AddSolutionMetricsViolations(violations);
        AddExternalCallsViolations(violations);
        AddCodeSimilarityViolations(violations);
        return violations;
    }

    private void AddConditionalStatementsViolations(List<Violation> violations)
    {
        if (IfFrequency != null || ForFrequency != null || ForEachFrequency != null || WhileFrequency != null)
        {
            violations.Add(new Violation
            {
                Type = ViolationType.ConditionalStatements,
                Description = "Overview of the conditional statement using across the solution",
                ExtendedData = new Dictionary<string, string>()
                {
                    {"If frequency", $"{IfFrequency}"},
                    {"For frequency", $"{ForFrequency}"},
                    {"For each frequency", $"{ForEachFrequency}"},
                    {"While frequency", $"{WhileFrequency}"}
                }
            });
        }
    }
    
    private void AddSolutionMetricsViolations(List<Violation> violations)
    {
        if (TotalClasses != null || TotalInterfaces != null || TotalMethods != null || TotalInheritanceDeclarations != null || TotalUsingDirectives != null)
        {
            violations.Add(new Violation
            {
                Type = ViolationType.SolutionMetrics,
                Description = "Overview of the solution metrics",
                ExtendedData = new Dictionary<string, string>()
                {
                    {"Total classes", $"{TotalClasses}"},
                    {"Total interfaces", $"{TotalInterfaces}"},
                    {"Total inheritance declarations", $"{TotalInheritanceDeclarations}"},
                    {"Total methods", $"{TotalMethods}"},
                    {"Total using declarations", $"{TotalUsingDirectives}"}
                }
            });
        }

        if (TotalCodeLines > 0 && CodeLinesPerFile != null)
        {
            var extendedData = CodeLinesPerFile
                .SelectMany(file => file)
                .ToDictionary(dict => dict.Key, dict => dict.Value.ToString());
            
            violations.Add(new Violation
            {
                Type = ViolationType.SolutionMetrics,
                Description = $"Total code lines: {TotalClasses}. Overview of the code lines per file",
                ExtendedData = extendedData
            });
        }
        
        if (TotalCommentLines > 0 && CommentLinesPerFile != null)
        {
            var extendedData = CommentLinesPerFile
                .SelectMany(file => file)
                .ToDictionary(dict => dict.Key, dict => dict.Value.ToString());
            
            violations.Add(new Violation
            {
                Type = ViolationType.SolutionMetrics,
                Description = $"Total comment lines: {TotalCommentLines}. Overview of the code lines per file",
                ExtendedData = extendedData
            });
        }

        if (ClassCouplings != null && ClassCouplings.Any())
        {
            var extendedData = ClassCouplings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
            violations.Add(new Violation
            {
                Type = ViolationType.SolutionMetrics,
                Description = $"Overview of the class coupling",
                ExtendedData = extendedData
            });
        }
    } 
    
    private void AddExternalCallsViolations(List<Violation> violations)
    {
        if (ExternalApiCalls != null && TotalHttpClientCalls != null)
        {
            var extendedData = new Dictionary<string, string>()
            {
                {"Total Http Client Calls", $"{TotalHttpClientCalls}"}
            };

            foreach (var apiCall in ExternalApiCalls)
            {
                extendedData.Add(apiCall.Key, apiCall.Value.ToString());
            }
            
            violations.Add(new Violation
            {
                Type = ViolationType.ExternalCalls,
                Description = $"Overview of the external calls across the solution",
                ExtendedData = extendedData
            });
        }

        if (EndOfLifeFrameworks != null)
        {
            violations.AddRange(
                from dict in EndOfLifeFrameworks
                    .SelectMany(keyValue => keyValue)
                where dict.Value.Status
                select new Violation
                {
                    Type = ViolationType.CodeSimilarity, 
                    Description = $"Framework with version \"{dict.Value.Version}\" is not longer supported on {dict.Key}"
                }
            );        
        }
    }
    
    private void AddCodeSimilarityViolations(List<Violation> violations)
    {
        if (CodeSimilarities == null || !CodeSimilarities.Any()) return;
        violations.AddRange(
            from keyValue in CodeSimilarities 
            let extendedData = keyValue.Value
                .ToDictionary(kvp => kvp.Key, kvp => $"{kvp.Value}") 
            select new Violation
            {
                Type = ViolationType.CodeSimilarity, 
                Description = $"Code similarities with file {keyValue.Key}",
                ExtendedData = extendedData
            });
    }
}