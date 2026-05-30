using System.Text.Json.Serialization;

namespace Application.DTOs.External;

public class TopArtistsDto
{
    [JsonPropertyName("artist")]
    public List<ArtistDto> Artists { get; set; }
}