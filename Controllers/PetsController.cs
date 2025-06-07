using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHotelCMS.Data;
using PetHotelCMS.DTOs;
using PetHotelCMS.Models;

namespace PetHotelCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDTO>>> GetPets()
        {
            return await _context.Pets
                .Select(p => new PetDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    OwnerId = p.OwnerId
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetDTO>> GetPet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null) return NotFound();

            return new PetDTO
            {
                Id = pet.Id,
                Name = pet.Name,
                Type = pet.Type,
                OwnerId = pet.OwnerId
            };
        }

        [HttpPost]
        public async Task<ActionResult<PetDTO>> CreatePet(PetDTO dto)
        {
            var pet = new Pet
            {
                Name = dto.Name,
                Type = dto.Type,
                OwnerId = dto.OwnerId
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            dto.Id = pet.Id;

            return CreatedAtAction(nameof(GetPet), new { id = pet.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(int id, PetDTO dto)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return NotFound();

            pet.Name = dto.Name;
            pet.Type = dto.Type;
            pet.OwnerId = dto.OwnerId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return NotFound();

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
