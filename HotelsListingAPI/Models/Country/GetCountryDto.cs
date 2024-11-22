using HotelsListingAPI.Models.Hotel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsListingAPI.Models.Country
{
    public class GetCountryDto : BaseCountryDto
    {
        public int CountryId { get; set; }
    }

    

    
}
