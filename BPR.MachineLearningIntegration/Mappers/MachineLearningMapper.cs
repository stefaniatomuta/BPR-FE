using AutoMapper;
using BPR.MachineLearningIntegration.Models;
using BPR.Model.Results;
using EndOfLifeFramework = BPR.Model.Results.EndOfLifeFramework;
using ExternalApiMetrics = BPR.Model.Results.ExternalApiMetrics;

namespace BPR.MachineLearningIntegration.Mappers;

public class MachineLearningMapper : Profile
{
    public MachineLearningMapper()
    {
        CreateMap<MLAnalysisResponseModel, ExtendedAnalysisResults>();
        CreateMap<Models.EndOfLifeFramework, EndOfLifeFramework>();
        CreateMap<Models.ExternalApiMetrics, ExternalApiMetrics>();
    }
}
