using AutoMapper;
using BPR.Persistence.Models;
using BPRBE.Services.Models;

namespace BPRBE.Services.Mappers;

public class ServiceMappers : Profile
{
    public ServiceMappers()
    {
        CreateMap<Rule, RuleCollection>().ReverseMap();
        CreateMap<ArchitecturalComponentCollection, ArchitecturalComponent>().ReverseMap();
        CreateMap<ArchitecturalModelCollection, ArchitecturalModel>().ReverseMap();
    }
}