using HotelsListingAPI.Core.Models.Country;

namespace HotelsListingAPI.Core.Models.Hotel
{
    public class GetHotelDetailsDto : BaseHotelDto
    {
        public int HotelId { get; set; }

        public string CountryName { get; set; }
    }
}
