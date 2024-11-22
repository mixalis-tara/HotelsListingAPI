using HotelsListingAPI.Data;

namespace HotelsListingAPI.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }
}
