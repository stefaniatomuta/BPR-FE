using AutoMapper;
using BPR.Analysis.Models;
using BPR.Mediator.Models;
using BPRBlazor.ViewModels;

namespace BPRBlazor.Mappers; 

public class MapperProfile : Profile {
    public MapperProfile() {
        CreateMap<AnalysisArchitecturalComponent, ArchitecturalComponentViewModel>().ReverseMap();
        CreateMap<AnalysisArchitecturalModel, ArchitecturalModelViewModel>().ReverseMap();
        CreateMap<AnalysisNamespace, NamespaceViewModel>().ReverseMap();
        CreateMap<Violation, ViolationModel>().ReverseMap();
    }
}