using BPRBE.Models.Results;
using BPRBE.Models.Results.Enums;

namespace BPRBlazor.Pages;

public partial class Results : ComponentBase
{
    private ResultModel? _resultModel;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _resultModel = GenerateResultModel();
    }

    private static ResultModel GenerateResultModel()
    {
        var score = new Random()
            .NextDouble() * 100;

        var violations = GenerateViolations();

        return new(score, violations);
    }

    private static List<ViolationModel> GenerateViolations()
    {
        return new List<ViolationModel>
        {
            new(1, "Violation name 1", ViolationType.ForbiddenDependencyDirection, "A class from the 'Models' namespace must not depend on a class in the 'Controllers' namespace", ViolationSeverity.Major, "using Controllers;\n\nnamespace Models;\n\npublic class One {\n    private IDependency _dependency;\n    \n    public One(IDependency dependency) {\n        _dependency = dependency;\n    }"),
            new(2, "Violation name 2", ViolationType.Unknown, "Some other thing I have no idea...", ViolationSeverity.Minor, "public class Two {\n    private IDependency _dependency;\n    \n    public One(IDependency dependency) {\n        _dependency = dependency;\n    }"),
            new(3, "Violation name 3", ViolationType.Unknown, "Some other thing I have no idea...", ViolationSeverity.Critical, "public class Two {\n    private IDependency _dependency;\n    \n    public One(IDependency dependency) {\n        _dependency = dependency;\n    }"),
        };
    }
}