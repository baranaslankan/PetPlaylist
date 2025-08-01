using Microsoft.AspNetCore.Mvc;
using PetPlaylist.Data;
using PetPlaylist.Models;
using PetPlaylist.DTOs;
using Microsoft.EntityFrameworkCore;

namespace PetPlaylist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PlaylistController(ApplicationDbContext context) => _context = context;

        public record PlaylistDto(int PlaylistId, string PlaylistName, List<int> SongIds);

        /// <summary>
        /// Returns all playlists with their associated songs.
        /// </summary>
        /// <returns>200 OK - List of PlaylistDto</returns>
        /// <example>
        /// GET: api/Playlist -> [{PlaylistDto}, ...]
        /// </example>
        [HttpGet]
        public IActionResult List()
        {
            var playlists = _context.Playlists
                .Include(p => p.PlaylistSongs)
                .Select(p => new PlaylistDto(
                    p.PlaylistId,
                    p.PlaylistName,
                    p.PlaylistSongs.Select(ps => ps.SongId).ToList()
                ))
                .ToList();

            return Ok(playlists);
        }

        /// <summary>
        /// Returns a single playlist by ID.
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <returns>200 OK or 404 Not Found</returns>
        /// <example>
        /// GET: api/Playlist/Find/1 -> {PlaylistDto}
        /// </example>
        [HttpGet("Find/{id}")]
        public IActionResult Find(int id)
        {
            var playlist = _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefault(p => p.PlaylistId == id);

            if (playlist == null) return NotFound();

            return Ok(new PlaylistDto(
                playlist.PlaylistId,
                playlist.PlaylistName,
                playlist.PlaylistSongs.Select(ps => ps.SongId).ToList()
            ));
        }

        /// <summary>
        /// Creates a new playlist with a list of song IDs.
        /// </summary>
        /// <param name="dto">Playlist data with song IDs</param>
        /// <returns>201 Created</returns>
        /// <example>
        /// POST: api/Playlist
        /// </example>
        [HttpPost]
        public IActionResult Create([FromBody] PlaylistDto dto)
        {
            if (!dto.SongIds.All(id => _context.Songs.Any(s => s.SongId == id)))
                return BadRequest("One or more song IDs are invalid.");

            var playlist = new Playlist
            {
                PlaylistName = dto.PlaylistName,
                PlaylistSongs = dto.SongIds.Select(id => new PlaylistSong
                {
                    SongId = id
                }).ToList()
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Find), new { id = playlist.PlaylistId },
                dto with { PlaylistId = playlist.PlaylistId });
        }

        /// <summary>
        /// Updates an existing playlist and its associated songs.
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <param name="dto">Updated playlist data</param>
        /// <returns>204 No Content</returns>
        /// <example>
        /// PUT: api/Playlist/2
        /// </example>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PlaylistDto dto)
        {
            var playlist = _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefault(p => p.PlaylistId == id);

            if (playlist == null) return NotFound();

            if (!dto.SongIds.All(id => _context.Songs.Any(s => s.SongId == id)))
                return BadRequest("One or more song IDs are invalid.");

            playlist.PlaylistName = dto.PlaylistName;
            playlist.PlaylistSongs.Clear();

            foreach (var songId in dto.SongIds)
            {
                playlist.PlaylistSongs.Add(new PlaylistSong { SongId = songId });
            }

            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deletes a playlist by ID.
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <returns>204 No Content</returns>
        /// <example>
        /// DELETE: api/Playlist/3
        /// </example>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var playlist = _context.Playlists.Find(id);
            if (playlist == null) return NotFound();

            _context.Playlists.Remove(playlist);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Gets all playlists recommended for a specific behavior.
        /// </summary>
        /// <param name="behaviorId">Behavior ID</param>
        /// <returns>200 OK - List of PlaylistDto</returns>
        /// <example>
        /// GET: api/Playlist/ForBehavior/1 -> [{PlaylistDto}, ...]
        /// </example>
        [HttpGet("ForBehavior/{behaviorId}")]
        public IActionResult GetPlaylistsForBehavior(int behaviorId)
        {
            var playlists = _context.BehaviorPlaylists
                .Where(bp => bp.BehaviorId == behaviorId)
                .Include(bp => bp.Playlist)
                    .ThenInclude(p => p.PlaylistSongs)
                .Select(bp => new PlaylistDto(
                    bp.Playlist.PlaylistId,
                    bp.Playlist.PlaylistName,
                    bp.Playlist.PlaylistSongs.Select(ps => ps.SongId).ToList()
                ))
                .ToList();

            return Ok(playlists);
        }

        /// <summary>
        /// Gets all recommended playlists for a specific pet based on their behaviors.
        /// </summary>
        /// <param name="petId">Pet ID</param>
        /// <returns>200 OK - List of PlaylistDto</returns>
        /// <example>
        /// GET: api/Playlist/ForPet/1 -> [{PlaylistDto}, ...]
        /// </example>
        [HttpGet("ForPet/{petId}")]
        public IActionResult GetRecommendedPlaylistsForPet(int petId)
        {
            // Get pet's behavior IDs
            var petBehaviorIds = _context.PetBehaviors
                .Where(pb => pb.PetId == petId)
                .Select(pb => pb.BehaviorId)
                .ToList();

            if (!petBehaviorIds.Any())
                return Ok(new List<PlaylistDto>()); // No behaviors recorded for this pet

            // Get playlists recommended for those behaviors
            var playlists = _context.BehaviorPlaylists
                .Where(bp => petBehaviorIds.Contains(bp.BehaviorId))
                .Include(bp => bp.Playlist)
                    .ThenInclude(p => p.PlaylistSongs)
                .Select(bp => bp.Playlist)
                .Distinct()
                .Select(p => new PlaylistDto(
                    p.PlaylistId,
                    p.PlaylistName,
                    p.PlaylistSongs.Select(ps => ps.SongId).ToList()
                ))
                .ToList();

            return Ok(playlists);
        }

        /// <summary>
        /// Associates a playlist with a behavior for recommendations.
        /// </summary>
        /// <param name="playlistId">Playlist ID</param>
        /// <param name="behaviorId">Behavior ID</param>
        /// <returns>201 Created</returns>
        /// <example>
        /// POST: api/Playlist/1/AssociateBehavior/2
        /// </example>
        [HttpPost("{playlistId}/AssociateBehavior/{behaviorId}")]
        public IActionResult AssociatePlaylistWithBehavior(int playlistId, int behaviorId)
        {
            // Check if playlist and behavior exist
            if (!_context.Playlists.Any(p => p.PlaylistId == playlistId))
                return NotFound("Playlist not found");
            
            if (!_context.Behaviors.Any(b => b.BehaviorId == behaviorId))
                return NotFound("Behavior not found");

            // Check if association already exists
            if (_context.BehaviorPlaylists.Any(bp => bp.PlaylistId == playlistId && bp.BehaviorId == behaviorId))
                return Conflict("Association already exists");

            var behaviorPlaylist = new BehaviorPlaylist
            {
                PlaylistId = playlistId,
                BehaviorId = behaviorId,
                CreatedDate = DateTime.UtcNow
            };

            _context.BehaviorPlaylists.Add(behaviorPlaylist);
            _context.SaveChanges();

            return Created($"api/Playlist/{playlistId}/AssociateBehavior/{behaviorId}", behaviorPlaylist);
        }

        /// <summary>
        /// Removes association between a playlist and a behavior.
        /// </summary>
        /// <param name="playlistId">Playlist ID</param>
        /// <param name="behaviorId">Behavior ID</param>
        /// <returns>204 No Content</returns>
        /// <example>
        /// DELETE: api/Playlist/1/AssociateBehavior/2
        /// </example>
        [HttpDelete("{playlistId}/AssociateBehavior/{behaviorId}")]
        public IActionResult RemovePlaylistBehaviorAssociation(int playlistId, int behaviorId)
        {
            var association = _context.BehaviorPlaylists
                .FirstOrDefault(bp => bp.PlaylistId == playlistId && bp.BehaviorId == behaviorId);

            if (association == null)
                return NotFound("Association not found");

            _context.BehaviorPlaylists.Remove(association);
            _context.SaveChanges();

            return NoContent();
        }
    }
}