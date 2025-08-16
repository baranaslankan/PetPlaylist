using Microsoft.EntityFrameworkCore;
using PetPlaylist.Data;
using PetPlaylist.Models;

namespace PetPlaylist.Services
{
    public class OwnerService
    {
        private readonly ApplicationDbContext _context;
        public OwnerService(ApplicationDbContext context) => _context = context;

        public async Task<Owner?> GetOwnerWithPetsAsync(int id)
        {
            return await _context.Owners.Include(o => o.Pets).FirstOrDefaultAsync(o => o.Id == id);
        }
        // Add more business logic methods as needed
    }
}
