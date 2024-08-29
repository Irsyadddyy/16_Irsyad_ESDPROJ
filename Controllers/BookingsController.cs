using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _16_Irsyad_ESDPROJ.Data;
using _16_Irsyad_ESDPROJ.Models;
using _16_Irsyad_ESDPROJ.Authorization;
using Microsoft.AspNetCore.Cors;

namespace _16_Irsyad_ESDPROJ.Controllers
{
    
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings(string facilityDescription = null)
        {
            IQueryable<Booking> query = _context.Bookings;

            if (!string.IsNullOrEmpty(facilityDescription))
            {
                query = query.Where(b => b.FacilityDescription.Contains(facilityDescription));
            }

            return await query.ToListAsync();
        }

        [HttpGet("TopFacilities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopFacilities()
        {
            var topFacilities = await _context.Facilities
                .OrderByDescending(f => f.Seatings)
                .Take(3)
                .ToListAsync();

            return Ok(topFacilities);
        }

        [HttpGet("AllFacilities")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Facility>>> GetAllFacilities(string facilityDescription = null, int? minSeats = null)
        {
            var query = _context.Facilities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(facilityDescription))
            {
                query = query.Where(f => f.FacilityName.Contains(facilityDescription) || f.Description.Contains(facilityDescription));
            }

            if (minSeats.HasValue)
            {
                query = query.Where(f => f.Seatings >= minSeats.Value);
            }

            var facilities = await query.ToListAsync();
            return Ok(facilities);
        }


        // GET: api/Bookings/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // POST: api/Bookings
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Booking>> PostBooking(CreateBookingDto bookingDto)
        {
            var booking = new Booking
            {
                FacilityDescription = bookingDto.FacilityDescription,
                BookingDateFrom = bookingDto.BookingDateFrom,
                BookingDateTo = bookingDto.BookingDateTo,
                BookedBy = bookingDto.BookedBy,
                BookingStatus = bookingDto.BookingStatus
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.BookingID }, booking);
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBooking(int id, BookingUpdateDto bookingDto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Update the booking with the DTO values
            booking.FacilityDescription = bookingDto.FacilityDescription;
            booking.BookingDateFrom = bookingDto.BookingDateFrom;
            booking.BookingDateTo = bookingDto.BookingDateTo;
            booking.BookedBy = bookingDto.BookedBy;
            booking.BookingStatus = bookingDto.BookingStatus;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // DELETE: api/Bookings/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }
    }
}