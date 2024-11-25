using AutoMapper;
using HotelsListingAPI.Contracts;
using HotelsListingAPI.Data;
using HotelsListingAPI.Models.Hotel;
using Microsoft.EntityFrameworkCore;

namespace HotelsListingAPI.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        private readonly HotelListingDbContext _context;

        public HotelsRepository(HotelListingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GetHotelDetailsDto> GetDetails(int id)
        {
            var hotel = await _context.Hotels.Include(q => q.Country)
                .FirstOrDefaultAsync(q => q.HotelId == id);

            if (hotel == null)
            {
                return null;
            }

            // Fetch the country name using CountryId
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == hotel.CountryId);

            var hotelDetailsDto = new GetHotelDetailsDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                Rating = hotel.Rating,
                CountryId = hotel.CountryId,
                CountryName = country?.Name // Ensure Country is not null
            };

            return hotelDetailsDto;
        }
    }
}
