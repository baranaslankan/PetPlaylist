using System.Collections.Generic;

namespace PetPlaylist.DTOs
{
    public class OwnerDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class OwnerWithPetsDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        // List of associated pets
        public List<PetDTO> Pets { get; set; } = new();
    }
}
