using System.Text.Json.Serialization;

namespace Application.DTOs.External;

public class ArtistDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}