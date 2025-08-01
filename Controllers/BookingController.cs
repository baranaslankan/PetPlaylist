using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPlaylist.Data;
using PetPlaylist.DTOs;
using PetPlaylist.Models;

namespace PetPlaylist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all bookings
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings()
        {
            return await _context.Bookings
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    PetId = b.PetId,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Notes = b.Notes
                })
                .ToListAsync();
        }

        /// <summary>
        /// Get a single booking by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            return new BookingDTO
            {
                Id = booking.Id,
                PetId = booking.PetId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Notes = booking.Notes
            };
        }

        /// <summary>
        /// Create a new booking
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BookingDTO>> CreateBooking(BookingDTO dto)
        {
            var booking = new Booking
            {
                PetId = dto.PetId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Notes = dto.Notes
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            dto.Id = booking.Id;
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, dto);
        }

        /// <summary>
        /// Update a booking
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, BookingDTO dto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.PetId = dto.PetId;
            booking.StartDate = dto.StartDate;
            booking.EndDate = dto.EndDate;
            booking.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete a booking
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
