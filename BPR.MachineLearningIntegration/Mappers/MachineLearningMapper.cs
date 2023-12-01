using AutoMapper;
using BPR.MachineLearningIntegration.Models;
using BPR.Model.Results;

namespace BPR.MachineLearningIntegration.Mappers;

public class MachineLearningMapper : Profile
{
    public MachineLearningMapper()
    {
        CreateMap<MLAnalysisResponseModel, ExtendedAnalysisResults>();
    }
}
