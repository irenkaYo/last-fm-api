using System.Text.Json.Serialization;

namespace Application.DTOs.External;

public class TopTracksDto
{
    [JsonPropertyName("track")]
    public List<TrackDto> Tracks { get; set; }
}