using System.Text.Json.Serialization;

namespace Application.DTOs.External.RecentTracks;

public class RecentTrackDateDto
{
    [JsonPropertyName("#text")]
    public string Text { get; set; }
}