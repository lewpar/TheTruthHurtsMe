using System.Text.Json.Serialization;

namespace TheTruthHurtsMe.Services.Models;

public class SpotifyArtist
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}