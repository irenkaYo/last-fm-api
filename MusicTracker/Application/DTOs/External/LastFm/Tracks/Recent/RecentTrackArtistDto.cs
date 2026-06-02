using System.Text.Json.Serialization;

namespace Application.DTOs.External.Recent;

public class RecentTrackArtistDto
{
    [JsonPropertyName("#text")]
    public string Name { get; set; }
}