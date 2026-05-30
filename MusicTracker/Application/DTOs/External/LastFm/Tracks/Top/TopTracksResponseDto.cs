using System.Text.Json.Serialization;

namespace Application.DTOs.External;

public class TopTracksResponseDto
{
    [JsonPropertyName("toptracks")]
    public TopTracksDto TopTracks { get; set; }
}