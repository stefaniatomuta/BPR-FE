using AutoMapper;
using BPR.Analysis.Models;
using BPR.Mediator.Models;
using BPR.Persistence.Models;

namespace BPR.Mediator.Mappers;

public class ServiceMappers : Profile
{
    public ServiceMappers()
    {
        CreateMap<Rule, RuleCollection>().ReverseMap();
        CreateMap<ArchitecturalComponentCollection, ArchitecturalComponent>().ReverseMap();
        CreateMap<ArchitecturalModelCollection, ArchitecturalModel>().ReverseMap();
        CreateMap<ArchitecturalComponent, AnalysisArchitecturalComponent>().ReverseMap();
        CreateMap<ArchitecturalModel, AnalysisArchitecturalModel>().ReverseMap();
        CreateMap<NamespaceModel, AnalysisNamespace>().ReverseMap();
        CreateMap<BPR.Persistence.Models.Violation, ViolationModel>().ReverseMap();
        CreateMap<BPR.Analysis.Models.Violation, ViolationModel>().ReverseMap();
        CreateMap<ResultModel, ResultCollection>().ReverseMap();
    }
}