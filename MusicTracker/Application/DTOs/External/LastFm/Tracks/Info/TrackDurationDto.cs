using System.Text.Json.Serialization;

namespace Application.DTOs.External.Info;

public class TrackDurationDto
{
    [JsonPropertyName("duration")]
    public string Duration { get; set; }
}