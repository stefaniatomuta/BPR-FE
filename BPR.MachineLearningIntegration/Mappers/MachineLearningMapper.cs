using AutoMapper;
using BPR.MachineLearningIntegration.Models;
using BPR.Model.Results.External;

namespace BPR.MachineLearningIntegration.Mappers;

public class MachineLearningMapper : Profile
{
    public MachineLearningMapper()
    {
        CreateMap<MLAnalysisResponseModel, ExtendedAnalysisResults>();
        CreateMap<Models.EndOfLifeFramework, Model.Results.External.EndOfLifeFramework>();
        CreateMap<Models.ExternalApiMetrics, Model.Results.External.ExternalApiMetrics>();
    }
}
