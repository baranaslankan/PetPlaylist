using Microsoft.EntityFrameworkCore;
using PetHotelCMS.Models;

namespace PetHotelCMS.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Owner> Owners { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Booking> Bookings { get; set; }
}
