using System.ComponentModel.DataAnnotations;

namespace PetPlaylist.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }
        public Pet? Pet { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
