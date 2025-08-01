namespace PetPlaylist.Models;

public class Owner
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public List<Pet>? Pets { get; set; }
}
