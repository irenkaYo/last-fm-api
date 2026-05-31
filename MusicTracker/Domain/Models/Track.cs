namespace Domain.Models;

public class Track
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public string ArtistName { get; set; }
}