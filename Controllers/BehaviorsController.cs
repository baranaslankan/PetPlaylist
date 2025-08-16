using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPlaylist.Data;
using PetPlaylist.Models;

namespace PetPlaylist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BehaviorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BehaviorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Simple DTOs
        public record BehaviorDto(int BehaviorId, string BehaviorName, string Description);
        public record CreateBehaviorDto(string BehaviorName, string Description);
        public record AssignBehaviorToPetDto(int PetId, int BehaviorId, string Notes = "");

    /// <summary>
    /// Gets all available behaviors.
    /// </summary>
    /// <remarks>
    /// <b>Example Request:</b>
    /// <code>GET /api/Behaviors</code>
    /// <b>Example Response:</b>
    /// <code>
    /// [
    ///   { "behaviorId": 1, "behaviorName": "Calm", "description": "Relaxed and quiet" },
    ///   ...
    /// ]
    /// </code>
    /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetBehaviors()
        {
            var behaviors = await _context.Behaviors
                .Select(b => new BehaviorDto(b.BehaviorId, b.BehaviorName, b.Description))
                .ToListAsync();

            return Ok(behaviors);
        }

    /// <summary>
    /// Creates a new behavior.
    /// </summary>
    /// <remarks>
    /// <b>Example Request:</b>
    /// <code>POST /api/Behaviors</code>
    /// <b>Example Body:</b>
    /// <code>
    /// { "behaviorName": "Calm", "description": "Relaxed and quiet" }
    /// </code>
    /// <b>Example Response:</b>
    /// <code>
    /// { "behaviorId": 1, "behaviorName": "Calm", "description": "Relaxed and quiet" }
    /// </code>
    /// </remarks>
        [HttpPost]
        public async Task<IActionResult> CreateBehavior(CreateBehaviorDto dto)
        {
            var behavior = new Behavior
            {
                BehaviorName = dto.BehaviorName,
                Description = dto.Description,
            };

            _context.Behaviors.Add(behavior);
            await _context.SaveChangesAsync();

            var result = new BehaviorDto(behavior.BehaviorId, behavior.BehaviorName, behavior.Description);
            return CreatedAtAction(nameof(GetBehavior), new { id = behavior.BehaviorId }, result);
        }

        /// <summary>
        /// Gets a specific behavior by ID.
        /// </summary>
        /// <param name="id">Behavior ID</param>
        /// <returns>200 OK or 404 Not Found</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBehavior(int id)
        {
            var behavior = await _context.Behaviors
                .Where(b => b.BehaviorId == id)
                .Select(b => new BehaviorDto(b.BehaviorId, b.BehaviorName, b.Description))
                .FirstOrDefaultAsync();

            if (behavior == null)
                return NotFound();

            return Ok(behavior);
        }

        /// <summary>
        /// Assigns a behavior to a pet.
        /// </summary>
        /// <param name="dto">Assignment data</param>
        /// <returns>201 Created</returns>
        [HttpPost("assign-to-pet")]
        public async Task<IActionResult> AssignBehaviorToPet(AssignBehaviorToPetDto dto)
        {
            // Check if pet and behavior exist
            if (!await _context.Pets.AnyAsync(p => p.Id == dto.PetId))
                return NotFound("Pet not found");

            if (!await _context.Behaviors.AnyAsync(b => b.BehaviorId == dto.BehaviorId))
                return NotFound("Behavior not found");

            // Check if already assigned
            if (await _context.PetBehaviors.AnyAsync(pb => pb.PetId == dto.PetId && pb.BehaviorId == dto.BehaviorId))
                return Conflict("Behavior already assigned to this pet");

            var petBehavior = new PetBehavior
            {
                PetId = dto.PetId,
                BehaviorId = dto.BehaviorId
            };

            _context.PetBehaviors.Add(petBehavior);
            await _context.SaveChangesAsync();

            return Created($"api/Behaviors/assign-to-pet", new { PetId = dto.PetId, BehaviorId = dto.BehaviorId });
        }

        /// <summary>
        /// Gets all behaviors assigned to a specific pet.
        /// </summary>
        /// <param name="petId">Pet ID</param>
        /// <returns>200 OK - List of behaviors for the pet</returns>
        [HttpGet("pet/{petId}")]
        public async Task<IActionResult> GetPetBehaviors(int petId)
        {
            var behaviors = await _context.PetBehaviors
                .Where(pb => pb.PetId == petId)
                .Include(pb => pb.Behavior)
                .Select(pb => new BehaviorDto(
                    pb.Behavior.BehaviorId,
                    pb.Behavior.BehaviorName,
                    pb.Behavior.Description
                ))
                .ToListAsync();

            return Ok(behaviors);
        }

        /// <summary>
        /// Removes a behavior assignment from a pet.
        /// </summary>
        /// <param name="petId">Pet ID</param>
        /// <param name="behaviorId">Behavior ID</param>
        /// <returns>204 No Content</returns>
        [HttpDelete("pet/{petId}/behavior/{behaviorId}")]
        public async Task<IActionResult> RemoveBehaviorFromPet(int petId, int behaviorId)
        {
            var petBehavior = await _context.PetBehaviors
                .FirstOrDefaultAsync(pb => pb.PetId == petId && pb.BehaviorId == behaviorId);

            if (petBehavior == null)
                return NotFound("Behavior assignment not found");

            _context.PetBehaviors.Remove(petBehavior);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
