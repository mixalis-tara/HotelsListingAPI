using HotelsListingAPI.Models.Country;

namespace HotelsListingAPI.Models.Hotel
{
    public class GetHotelDetailsDto : BaseHotelDto
    {
        public int HotelId { get; set; }

        public string CountryName { get; set; }
    }
}
