using AutoMapper;
using BPR.Mediator.Models;
using BPRBlazor.ViewModels;

namespace BPRBlazor.Mappers; 

public class MapperProfile : Profile {
    public MapperProfile() {
        CreateMap<ArchitecturalComponent, ArchitecturalComponentViewModel>().ReverseMap();
        CreateMap<ArchitecturalModel, ArchitecturalModelViewModel>().ReverseMap();
        CreateMap<NamespaceViewModel, NamespaceModel>().ReverseMap();
        CreateMap<RuleViewModel, Rule>().ReverseMap();
        CreateMap<ViolationModel, ViolationViewModel>().ReverseMap();
        CreateMap<ResultModel, ResultViewModel>().ReverseMap();
    }
}