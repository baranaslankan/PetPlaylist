namespace PetHotelCMS.Models;

public class Booking
{
    public int Id { get; set; }
    public int PetId { get; set; }
    public Pet Pet { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Room { get; set; }
}
