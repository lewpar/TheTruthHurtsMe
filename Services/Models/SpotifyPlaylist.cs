using System.Text.Json.Serialization;

namespace TheTruthHurtsMe.Services.Models;

public class SpotifyPlaylist
{
    [JsonPropertyName("items")]
    public List<SpotifyPlaylistItem>? Items { get; set; }
}