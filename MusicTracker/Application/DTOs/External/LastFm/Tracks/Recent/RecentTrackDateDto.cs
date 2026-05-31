using System.Text.Json.Serialization;

namespace Application.DTOs.External.RecentTracks;

public class RecentTrackDateDto
{
    [JsonPropertyName("uts")]
    public string Uts { get; set; }
}