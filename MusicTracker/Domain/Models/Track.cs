namespace Domain.Models;

public class Track
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TimeSpan Duration { get; set; }
    public string ArtistName { get; set; }

    public Track(string name, TimeSpan duration, string artistName)
    {
        Id = Guid.NewGuid();
        Name = name;
        Duration = duration;
        ArtistName = artistName;
    }
}