namespace PetPlaylist.Models
{
    public class PetBehavior
    {
        public int PetId { get; set; }
        public Pet Pet { get; set; } = null!;
        
        public int BehaviorId { get; set; }
        public Behavior Behavior { get; set; } = null!;
        
    }
}
