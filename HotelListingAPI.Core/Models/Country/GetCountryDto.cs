using HotelsListingAPI.Core.Models.Hotel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsListingAPI.Core.Models.Country
{
    public class GetCountryDto : BaseCountryDto
    {
        public int CountryId { get; set; }
    }

    

    
}
