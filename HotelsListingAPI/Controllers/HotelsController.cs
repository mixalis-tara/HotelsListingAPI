using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelsListingAPI.Data;
using HotelsListingAPI.Contracts;
using AutoMapper;
using HotelsListingAPI.Models.Hotel;
using HotelsListingAPI.Models.Country;
using System.Diagnostics.Metrics;

namespace HotelsListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _hotelsRepository;
        private readonly IMapper _mapper;

        public HotelsController(IHotelsRepository hotelsRepository, IMapper mapper)
        {
            _hotelsRepository = hotelsRepository;
            _mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _hotelsRepository.GetAllAsync();
            return Ok(_mapper.Map<List<HotelDto>>(hotels));
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelDetailsDto>> GetHotel(int id)
        {
            var hotelDetails = await _hotelsRepository.GetDetails(id);

            if (hotelDetails == null)
            {
                return NotFound();
            }

            var countryDto = _mapper.Map<GetHotelDetailsDto>(hotelDetails);

            return Ok(countryDto);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.HotelId)
            {
                return BadRequest();
            }

            var hotel = await _hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            _mapper.Map(hotelDto, hotel);

            try
            {
                await _hotelsRepository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto createHotelDto)
        {
            var hotel = _mapper.Map<Hotel>(createHotelDto);
            await _hotelsRepository.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.HotelId }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _hotelsRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _hotelsRepository.Exists(id);
        }
    }
}
