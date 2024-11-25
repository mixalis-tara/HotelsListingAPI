using AutoMapper;
using HotelsListingAPI.Data;
using HotelsListingAPI.Models.Country;
using HotelsListingAPI.Models.Hotel;

namespace HotelsListingAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDetailsDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, GetHotelDetailsDto>().ReverseMap();

        }
    }
}
