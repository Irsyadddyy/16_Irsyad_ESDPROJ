using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _16_Irsyad_ESDPROJ.Data;
using _16_Irsyad_ESDPROJ.Models;
using _16_Irsyad_ESDPROJ.Authorization;

namespace _16_Irsyad_ESDPROJ.Controllers
{
    [Authorize]
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
        public IActionResult GetTopFacilities()
        {
            var topFacilities = new List<object>
    {
        new
        {
            FacilityID = 1,
            FacilityName = "Grand Conference Hall",
            Description = "The spacious and well-designed conference room features advanced audiovisual equipment, comfortable ergonomic seating, and adaptable lighting, " +
            "all arranged to create an ideal setting for productive meetings and collaborative sessions.",
            Seatings = 250
        },
        new
        {
            FacilityID = 2,
            FacilityName = "Cozy Meeting Room",
            Description = "A warm and inviting meeting room designed to accommodate smaller groups, " +
            "offering a comfortable and intimate environment where participants can engage in meaningful discussions and collaborate effectively.",
            Seatings = 10
        },
        new
        {
            FacilityID = 3,
            FacilityName = "Scenic Outdoor Plaza",
            Description = "An expansive outdoor event space that boasts a captivating and picturesque view, " +
            "offering a scenic and enchanting backdrop that elevates the atmosphere and charm of any gathering, whether it's a wedding, corporate event, or casual celebration.",
            Seatings = 300
        }
    };
            return Ok(topFacilities);
        }


        // GET: api/Bookings/5
        [HttpGet("{id}")]
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