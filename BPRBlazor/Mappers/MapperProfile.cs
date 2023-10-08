using AutoMapper;
using BPR.Analysis.Models;
using BPR.Models.Blazor;
using BPRBlazor.Models;


namespace BPRBlazor.Mappers; 

public class MapperProfile : Profile {
    public MapperProfile() {
        CreateMap<AnalysisArchitecturalComponent, ArchitecturalComponentViewModel>().ReverseMap();
        CreateMap<AnalysisArchitecturalModel, ArchitecturalModelViewModel>().ReverseMap();
        CreateMap<AnalysisNamespace, NamespaceViewModel>().ReverseMap();
    }
}