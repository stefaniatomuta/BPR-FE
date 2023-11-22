using AutoMapper;
using BPR.Model.Architectures;
using BPR.Model.Results;
using BPR.Persistence.Collections;

namespace BPR.Persistence.Utils;

public class ServiceMappers : Profile
{
    public ServiceMappers()
    {
        CreateMap<Rule, RuleCollection>().ReverseMap();
        CreateMap<ArchitecturalModelCollection, ArchitecturalModel>().ReverseMap();
        CreateMap<AnalysisResult, ResultCollection>().ReverseMap();
    }
}