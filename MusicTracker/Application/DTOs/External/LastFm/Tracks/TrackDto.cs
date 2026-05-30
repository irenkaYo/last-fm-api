using System.Text.Json.Serialization;

namespace Application.DTOs.External;

public class TrackDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("artist")]
    public ArtistDto Artist { get; set; }
}