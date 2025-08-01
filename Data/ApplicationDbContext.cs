using Microsoft.EntityFrameworkCore;
using PetPlaylist.Models;

namespace PetPlaylist.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Pet Management
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    // Music & Playlist Management
    public DbSet<Song> Songs { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Artist> Artists { get; set; }
    
    // Behavior & Playlist Relationships
    public DbSet<Behavior> Behaviors { get; set; }
    public DbSet<PetBehavior> PetBehaviors { get; set; }
    public DbSet<BehaviorPlaylist> BehaviorPlaylists { get; set; }
    
    // Junction Tables
    public DbSet<PlaylistSong> PlaylistSongs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many relationships
        
        // PlaylistSong (Playlist <-> Song)
        modelBuilder.Entity<PlaylistSong>()
            .HasKey(ps => new { ps.PlaylistId, ps.SongId });
        
        modelBuilder.Entity<PlaylistSong>()
            .HasOne(ps => ps.Playlist)
            .WithMany(p => p.PlaylistSongs)
            .HasForeignKey(ps => ps.PlaylistId);
        
        modelBuilder.Entity<PlaylistSong>()
            .HasOne(ps => ps.Song)
            .WithMany(s => s.PlaylistSongs)
            .HasForeignKey(ps => ps.SongId);

        // PetBehavior (Pet <-> Behavior)
        modelBuilder.Entity<PetBehavior>()
            .HasKey(pb => new { pb.PetId, pb.BehaviorId });
        
        modelBuilder.Entity<PetBehavior>()
            .HasOne(pb => pb.Pet)
            .WithMany(p => p.PetBehaviors)
            .HasForeignKey(pb => pb.PetId);
        
        modelBuilder.Entity<PetBehavior>()
            .HasOne(pb => pb.Behavior)
            .WithMany(b => b.PetBehaviors)
            .HasForeignKey(pb => pb.BehaviorId);

        // BehaviorPlaylist (Behavior <-> Playlist)
        modelBuilder.Entity<BehaviorPlaylist>()
            .HasKey(bp => new { bp.BehaviorId, bp.PlaylistId });
        
        modelBuilder.Entity<BehaviorPlaylist>()
            .HasOne(bp => bp.Behavior)
            .WithMany(b => b.BehaviorPlaylists)
            .HasForeignKey(bp => bp.BehaviorId);
        
        modelBuilder.Entity<BehaviorPlaylist>()
            .HasOne(bp => bp.Playlist)
            .WithMany(p => p.BehaviorPlaylists)
            .HasForeignKey(bp => bp.PlaylistId);

        // Other relationships
        modelBuilder.Entity<Pet>()
            .HasOne(p => p.Owner)
            .WithMany(o => o.Pets)
            .HasForeignKey(p => p.OwnerId);

        modelBuilder.Entity<Song>()
            .HasOne(s => s.Artist)
            .WithMany(a => a.Songs)
            .HasForeignKey(s => s.ArtistId);
    }
}
