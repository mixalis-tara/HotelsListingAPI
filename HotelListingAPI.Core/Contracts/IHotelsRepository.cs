using HotelsListingAPI.Data;
using HotelsListingAPI.Core.Models.Hotel;

namespace HotelsListingAPI.Core.Contracts
{
    public interface IHotelsRepository : IGenericRepository<Hotel>
    {
        Task<GetHotelDetailsDto> GetDetails(int id);
    }
}
