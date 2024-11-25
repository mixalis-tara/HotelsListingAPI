using HotelsListingAPI.Data;
using HotelsListingAPI.Models.Hotel;

namespace HotelsListingAPI.Contracts
{
    public interface IHotelsRepository : IGenericRepository<Hotel>
    {
        Task<GetHotelDetailsDto> GetDetails(int id);
    }
}
