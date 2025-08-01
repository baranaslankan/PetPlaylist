using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPlaylist.Data;
using PetPlaylist.DTOs;
using PetPlaylist.Models;

namespace PetPlaylist.Controllers
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

        /// <summary>
        /// Returns all pets.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDTO>>> GetPets()
        {
            return await _context.Pets
                .Select(p => new PetDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    Age = p.Age,
                    OwnerId = p.OwnerId
                })
                .ToListAsync();
        }

        /// <summary>
        /// Returns a specific pet by ID.
        /// </summary>
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
                Breed = pet.Breed,
                Age = pet.Age,
                OwnerId = pet.OwnerId
            };
        }

        /// <summary>
        /// Creates a new pet.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PetDTO>> CreatePet(PetDTO dto)
        {
            var pet = new Pet
            {
                Name = dto.Name,
                Type = dto.Type,
                Breed = dto.Breed,
                Age = dto.Age,
                OwnerId = dto.OwnerId
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            dto.Id = pet.Id;
            return CreatedAtAction(nameof(GetPet), new { id = pet.Id }, dto);
        }

        /// <summary>
        /// Updates an existing pet.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(int id, PetDTO dto)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return NotFound();

            pet.Name = dto.Name;
            pet.Type = dto.Type;
            pet.Breed = dto.Breed;
            pet.Age = dto.Age;
            pet.OwnerId = dto.OwnerId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a pet by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return NotFound();

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns all pets owned by a specific owner.
        /// </summary>
        [HttpGet("/api/owners/{ownerId}/pets")]
        public async Task<ActionResult<IEnumerable<PetDTO>>> GetPetsByOwner(int ownerId)
        {
            var pets = await _context.Pets
                .Where(p => p.OwnerId == ownerId)
                .Select(p => new PetDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    Age = p.Age,
                    OwnerId = p.OwnerId
                })
                .ToListAsync();

            return pets;
        }
    }
}
