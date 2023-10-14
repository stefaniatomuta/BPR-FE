using AutoMapper;
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
    }
}