using AutoMapper;
using HotelsListingAPI.Core.Contracts;
using HotelsListingAPI.Data;
using HotelsListingAPI.Core.Models.Hotel;
using Microsoft.EntityFrameworkCore;

namespace HotelsListingAPI.Core.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public HotelsRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
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
