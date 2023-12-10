using BPR.Model.Results;

namespace BPRBlazor.Services;

public class ExtendedAnalysisResultsHandler
{
    public string? HandleTechnicalDebtClassification(ExtendedAnalysisResults results)
    {
        return results.TechnicalDebtClassification;
    }

    public Dictionary<string, double>? HandleConditionalStatements(ExtendedAnalysisResults results)
    {
        if (results.IfFrequency == null
            || results.ForFrequency == null
            || results.ForEachFrequency == null
            || results.WhileFrequency == null
           )
        {
            return null;
        }

        return new Dictionary<string, double>
        {
            {"If", results.IfFrequency.Value},
            {"For", results.ForFrequency.Value},
            {"ForEach", results.ForEachFrequency.Value},
            {"While", results.WhileFrequency.Value}
        };
    }

    public Dictionary<string, int>? HandleSolutionMetrics(ExtendedAnalysisResults results)
    {
        if (results.TotalClasses == null
            || results.TotalCSharpFiles == null
            || results.TotalInterfaces == null
            || results.TotalMethods == null
            || results.TotalInheritanceDeclarations == null
            || results.TotalUsingDirectives == null
           )
        {
            return null;
        }

        return new Dictionary<string, int>
        {
            {"Classes", results.TotalClasses.Value},
            {"C# files", results.TotalCSharpFiles.Value},
            {"Interfaces", results.TotalInterfaces.Value},
            {"Methods", results.TotalMethods.Value},
            {"Inheritance declarations", results.TotalInheritanceDeclarations.Value},
            {"Using directives", results.TotalUsingDirectives.Value},
        };
    }

    public Dictionary<string, int>? HandleCodeLinesPerFile(ExtendedAnalysisResults results)
    {
        if (results.TotalCodeLines == null)
        {
            return null;
        }

        return results.CodeLinesPerFile?.SelectMany(file => file)
            .OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public Dictionary<string, int>? HandleCommentLinesPerFile(ExtendedAnalysisResults results)
    {
        if (results.TotalCommentLines == null)
        {
            return null;
        }

        return results.CommentLinesPerFile?.SelectMany(file => file)
            .OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public Dictionary<string, double>? HandleCodeLinesMetrics(ExtendedAnalysisResults results)
    {
        if (results.TotalCodeLines == null
            || results.TotalCommentLines == null
           )
        {
            return null;
        }

        return new Dictionary<string, double>
        {
            {"Lines of code", results.TotalCodeLines.Value - results.TotalCommentLines.Value},
            {"Lines of comments", results.TotalCommentLines.Value}
        };
    }

    public Dictionary<string, ExternalApiMetrics>? HandleExternalApiCalls(ExtendedAnalysisResults results)
    {
        if ((results.ExternalApiCalls == null || !results.ExternalApiCalls.Any()) &&
            results.TotalHttpClientCalls == null)
        {
            return null;
        }

        if (results.TotalHttpClientCalls == null)
        {
            return results.ExternalApiCalls;
        }

        var dictionary = results.ExternalApiCalls ?? new Dictionary<string, ExternalApiMetrics>();
        dictionary.Add("HTTP client", new()
        { 
            Usage = results.TotalHttpClientCalls.Value
        });

        return dictionary
            .OrderByDescending(dict => dict.Value.Usage)
            .ToDictionary(dict => dict.Key, dict => dict.Value);
    }

    public Dictionary<string, int>? HandleClassCoupling(ExtendedAnalysisResults results)
    {
        if (results.ClassCouplings == null || !results.ClassCouplings.Any())
        {
            return null;
        }

        return results.ClassCouplings
            .OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public Dictionary<string, Dictionary<string, double>>? HandleCodeSimilarities(ExtendedAnalysisResults results)
    {
        if (results.CodeSimilarities == null || !results.CodeSimilarities.Any())
        {
            return null;
        }

        return results.CodeSimilarities.OrderByDescending(kvp => kvp.Value.Values.Sum())
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value
                .OrderByDescending(keyValuePair => keyValuePair.Value)
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value));
    }
}