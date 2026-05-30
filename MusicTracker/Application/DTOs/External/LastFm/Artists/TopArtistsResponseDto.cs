using System.Text.Json.Serialization;

namespace Application.DTOs.External;

public class TopArtistsResponseDto
{
    [JsonPropertyName("topartists")]
    public TopArtistsDto TopArtists { get; set; }
}