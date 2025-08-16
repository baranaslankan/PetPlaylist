using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PetPlaylist.Data;
using PetPlaylist.DTOs;
using PetPlaylist.Models;

namespace PetPlaylist.Controllers
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

    /// Returns all owners.
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

    /// Returns a specific owner by ID.
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

    /// Returns a specific owner and all their pets.
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

    /// Creates a new owner.
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
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

    /// Updates an existing owner.
        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOnly")]
        public async Task<IActionResult> UpdateOwner(int id, OwnerDTO dto)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null) return NotFound();

            // Only allow if the logged-in user matches the owner email
            var userEmail = User.Identity?.Name;
            if (User.IsInRole("Owner") && owner.Email != userEmail)
                return Forbid();

            owner.FullName = dto.FullName;
            owner.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

    /// Deletes an owner by ID.
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
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
