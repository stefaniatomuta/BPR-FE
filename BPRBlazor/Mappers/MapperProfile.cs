using AutoMapper;
using BPR.Model.Architectures;
using BPR.Model.Results;
using BPRBlazor.ViewModels;

namespace BPRBlazor.Mappers; 

public class MapperProfile : Profile {
    public MapperProfile() {
        CreateMap<ArchitecturalComponent, ArchitecturalComponentViewModel>().ReverseMap();
        CreateMap<ArchitecturalModel, ArchitecturalModelViewModel>().ReverseMap();
        CreateMap<NamespaceViewModel, NamespaceModel>().ReverseMap();
        CreateMap<RuleViewModel, Rule>().ReverseMap();
        CreateMap<Violation, ViolationViewModel>().ReverseMap();
        CreateMap<AnalysisResult, ResultViewModel>().ReverseMap();
        CreateMap<Position, PositionViewModel>().ReverseMap();
        CreateMap<DependencyViewModel, ArchitecturalDependency>().ReverseMap();
    }
}