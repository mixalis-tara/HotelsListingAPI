﻿using HotelsListingAPI.Contracts;
using HotelsListingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelsListingAPI.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _context;

        public CountriesRepository(HotelListingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries.Include(q => q.Hotels)
                       .FirstOrDefaultAsync(q => q.CountryId == id);
        }

        
    }
}