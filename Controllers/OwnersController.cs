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

        /// <summary>
        /// Returns all owners.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OwnerDTO>>> GetOwners()
        {
            return await _context.Owners
                .Select(o => new OwnerDTO
                {
                    Id = o.Id,
                    FullName = o.FullName,
                    Email = o.Email,
                    PhoneNumber = o.PhoneNumber,
                    Address = o.Address
                })
                .ToListAsync();
        }

        /// <summary>
        /// Returns a specific owner by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDTO>> GetOwner(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null) return NotFound();

            return new OwnerDTO
            {
                Id = owner.Id,
                FullName = owner.FullName,
                Email = owner.Email,
                PhoneNumber = owner.PhoneNumber,
                Address = owner.Address
            };
        }

        /// <summary>
        /// Returns a specific owner and all their pets.
        /// </summary>
        [HttpGet("{id}/with-pets")]
        public async Task<ActionResult<OwnerWithPetsDTO>> GetOwnerWithPets(int id)
        {
            var owner = await _context.Owners
                .Include(o => o.Pets)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (owner == null) return NotFound();

            return new OwnerWithPetsDTO
            {
                Id = owner.Id,
                FullName = owner.FullName,
                Email = owner.Email,
                PhoneNumber = owner.PhoneNumber,
                Address = owner.Address,
                Pets = owner.Pets.Select(p => new PetDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    Age = p.Age,
                    OwnerId = p.OwnerId
                }).ToList()
            };
        }

        /// <summary>
        /// Creates a new owner.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OwnerDTO>> CreateOwner(OwnerDTO dto)
        {
            var owner = new Owner
            {
                FullName = dto.FullName,
                Email = dto.Email
            };

            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();

            dto.Id = owner.Id;
            return CreatedAtAction(nameof(GetOwner), new { id = owner.Id }, dto);
        }

        /// <summary>
        /// Updates an existing owner.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(int id, OwnerDTO dto)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null) return NotFound();

            owner.FullName = dto.FullName;
            owner.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes an owner by ID.
        /// </summary>
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
