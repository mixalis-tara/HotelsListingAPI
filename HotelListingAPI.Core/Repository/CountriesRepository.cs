using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelsListingAPI.Core.Contracts;
using HotelsListingAPI.Core.Exceptions;
using HotelsListingAPI.Core.Models.Country;
using HotelsListingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelsListingAPI.Core.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetCountryDetailsDto> GetDetails(int id)
        {
            var country = await _context.Countries.Include(q =>q.Hotels)
                .ProjectTo<GetCountryDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q => q.CountryId == id);
            if(country == null)
            {
                throw new NotFoundException(nameof(GetDetails), id);
            }
            return country;
        }

        
    }
}
