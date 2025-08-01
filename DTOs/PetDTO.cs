namespace PetPlaylist.DTOs
{
    public class PetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Breed { get; set; } = "";
        public int Age { get; set; }
        public int OwnerId { get; set; }
    }

    public class CreatePetDTO
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Breed { get; set; } = "";
        public int Age { get; set; }
        public int OwnerId { get; set; }
    }
}
