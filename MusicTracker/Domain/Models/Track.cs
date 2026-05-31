namespace Domain.Models;

public class Track
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public string ArtistName { get; set; }

    public Track(string name, int duration, string artistName)
    {
        Id = Guid.NewGuid();
        Name = name;
        Duration = duration;
        ArtistName = artistName;
    }
}