using HotelsListingAPI.Models.Hotel;

namespace HotelsListingAPI.Models.Country
{
    public class GetCountryDetailsDto : BaseCountryDto
    {
        public int CountryId { get; set; }

        public List<HotelDto> Hotels { get; set; }
    }
}
