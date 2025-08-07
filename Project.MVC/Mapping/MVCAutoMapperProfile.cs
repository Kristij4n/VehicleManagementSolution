using AutoMapper;
using Project.Service_.DTOs;
using Project.MVC_.ViewModels;

namespace Project.MVC_.Mapping
{
    public class MVCAutoMapperProfile : Profile
    {
        public MVCAutoMapperProfile()
        {
            // VehicleMake mappings
            CreateMap<VehicleMakeDto, VehicleMakeViewModel>().ReverseMap();

            // VehicleModel mappings
            CreateMap<VehicleModelDto, VehicleModelViewModel>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.MakeName) ? src.MakeName :
                    (src.Make != null ? src.Make.Name : string.Empty)
                ))
                .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId));
            // Solve problem with empty MakeName
            CreateMap<VehicleModelViewModel, VehicleModelDto>()
                .ForMember(dest => dest.Make, opt => opt.Ignore()) // ignore navigation
                .ForMember(dest => dest.MakeName, opt => opt.Ignore()) // ignore display only
                .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId));
        }
    }
}