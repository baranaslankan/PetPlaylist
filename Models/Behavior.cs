namespace PetPlaylist.Models
{
    public class Behavior
    {
        public int BehaviorId { get; set; }
        public string BehaviorName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public ICollection<PetBehavior> PetBehaviors { get; set; } = new List<PetBehavior>();
        public ICollection<BehaviorPlaylist> BehaviorPlaylists { get; set; } = new List<BehaviorPlaylist>();
    }
}
