using HotelsListingAPI.Core.Models.Country;
using HotelsListingAPI.Data;

namespace HotelsListingAPI.Core.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<GetCountryDetailsDto> GetDetails(int id);
    }
}
