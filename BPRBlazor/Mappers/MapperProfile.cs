using AutoMapper;
using BPR.Model.Architectures;
using BPR.Model.Results;
using BPRBlazor.ViewModels;

namespace BPRBlazor.Mappers; 

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ArchitectureComponent, ArchitectureComponentViewModel>().ReverseMap();
        CreateMap<ArchitectureModel, ArchitectureModelViewModel>().ReverseMap();
        CreateMap<NamespaceViewModel, NamespaceModel>().ReverseMap();
        CreateMap<RuleViewModel, Rule>().ReverseMap();
        CreateMap<Violation, ViolationViewModel>().ReverseMap();
        CreateMap<AnalysisResult, ResultViewModel>().ReverseMap();
        CreateMap<Position, PositionViewModel>().ReverseMap();
        CreateMap<DependencyViewModel, ArchitectureDependency>().ReverseMap();
    }
}