using HotelsListingAPI.Core.Models.Hotel;

namespace HotelsListingAPI.Core.Models.Country
{
    public class GetCountryDetailsDto : BaseCountryDto
    {
        public int CountryId { get; set; }

        public List<HotelDto> Hotels { get; set; }
    }
}
