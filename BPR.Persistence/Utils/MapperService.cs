using AutoMapper;
using BPR.Model.Architectures;
using BPR.Model.Results;
using BPR.Model.Rules;
using BPR.Persistence.Collections;

namespace BPR.Persistence.Utils;

public class ServiceMappers : Profile
{
    public ServiceMappers()
    {
        CreateMap<Rule, RuleCollection>().ReverseMap();
        CreateMap<ArchitectureModelsCollection, ArchitectureModel>().ReverseMap();
        CreateMap<AnalysisResult, ResultCollection>().ReverseMap();
    }
}