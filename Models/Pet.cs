using System.Text.Json.Serialization;

namespace PetHotelCMS.Models;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Breed { get; set; }

    public int OwnerId { get; set; }

    [JsonIgnore]
    public Owner? Owner { get; set; }

}
