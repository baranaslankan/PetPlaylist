namespace PetHotelCMS.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }
    }
}
