using System.Text.Json.Serialization;

namespace TheTruthHurtsMe.Services.Models;

public class SpotifyTrack
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("artists")]
    public List<SpotifyArtist>? Artists { get; set; }
}