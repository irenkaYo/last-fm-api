using System.Text.Json.Serialization;
using Application.DTOs.External.Recent;

namespace Application.DTOs.External.RecentTracks;

public class RecentTrackDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("artist")]
    public RecentTrackArtistDto Artist { get; set; }
}