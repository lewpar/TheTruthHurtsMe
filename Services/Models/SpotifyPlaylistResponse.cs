using System.Text.Json.Serialization;

namespace TheTruthHurtsMe.Services.Models;

public class SpotifyPlaylistResponse
{
    [JsonPropertyName("tracks")]
    public SpotifyPlaylist? Tracks { get; set; }
}