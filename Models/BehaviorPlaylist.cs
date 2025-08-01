namespace PetPlaylist.Models
{
    public class BehaviorPlaylist
    {
        public int BehaviorId { get; set; }
        public Behavior Behavior { get; set; } = null!;
        
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
    }
}
