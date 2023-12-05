using BPR.Model.Results;

namespace BPRBlazor.Services;

public class ExtendedAnalysisResultsHandler
{
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
            { "If", results.IfFrequency.Value },
            { "For", results.ForFrequency.Value },
            { "ForEach", results.ForEachFrequency.Value },
            { "While", results.WhileFrequency.Value }
        };
    }

    public Dictionary<string, int>? HandleSolutionMetrics(ExtendedAnalysisResults results)
    {
        if (results.TotalClasses == null 
            || results.TotalInterfaces == null 
            || results.TotalMethods == null 
            || results.TotalInheritanceDeclarations == null 
            || results.TotalUsingDirectives == null 
            || results.TotalCodeLines == null 
            || results.TotalCommentLines == null
        )
        {
            return null;
        }

        return new Dictionary<string, int>
        {
            { "Classes", results.TotalClasses.Value },
            { "Interfaces", results.TotalInterfaces.Value },
            { "Methods", results.TotalMethods.Value },
            { "Inheritance declarations", results.TotalInheritanceDeclarations.Value },
            { "Using directives", results.TotalUsingDirectives.Value },
            { "Lines of code", results.TotalCodeLines.Value },
            { "Lines of comments", results.TotalCommentLines.Value }
        };
    }
    
    public Dictionary<string, int>? HandleExternalApiCalls(ExtendedAnalysisResults results)
    {
        if ((results.ExternalApiCalls == null || !results.ExternalApiCalls.Any()) && results.TotalHttpClientCalls == null)
        {
            return null;
        }

        if (results.TotalHttpClientCalls == null)
        {
            return results.ExternalApiCalls;
        }
        
        var dictionary = results.ExternalApiCalls ?? new Dictionary<string, int>();
        dictionary.Add("Http Client", results.TotalHttpClientCalls.Value);
        return dictionary;
    }
}
