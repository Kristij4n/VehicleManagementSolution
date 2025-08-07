using AutoMapper;
using Project.Service_.Models;
using Project.Service_.DTOs;

namespace Project.Service_.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VehicleMake, VehicleMakeDto>().ReverseMap();

            CreateMap<VehicleModel, VehicleModelDto>()
                .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Make)) // For reading, map Make navigation property
                .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId))
                .ReverseMap()
                .ForMember(dest => dest.Make, opt => opt.Ignore()) // For writing, ignore navigation property!
                .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId));
        }
    }
}