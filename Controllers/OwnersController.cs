using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHotelCMS.Data;
using PetHotelCMS.DTOs;
using PetHotelCMS.Models;

namespace PetHotelCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OwnersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OwnerDTO>>> GetOwners()
        {
            return await _context.Owners
                .Select(o => new OwnerDTO
                {
                    Id = o.Id,
                    Name = o.Name,
                    Email = o.Email
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDTO>> GetOwner(int id)
        {
            var owner = await _context.Owners.FindAsync(id);

            if (owner == null) return NotFound();

            return new OwnerDTO
            {
                Id = owner.Id,
                Name = owner.Name,
                Email = owner.Email
            };
        }

        [HttpPost]
        public async Task<ActionResult<OwnerDTO>> CreateOwner(OwnerDTO dto)
        {
            var owner = new Owner
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();

            dto.Id = owner.Id;

            return CreatedAtAction(nameof(GetOwner), new { id = owner.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(int id, OwnerDTO dto)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null) return NotFound();

            owner.Name = dto.Name;
            owner.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null) return NotFound();

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
