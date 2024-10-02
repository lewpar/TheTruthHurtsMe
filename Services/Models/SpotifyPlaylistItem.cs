using System.Text.Json.Serialization;

namespace TheTruthHurtsMe.Services.Models;

public class SpotifyPlaylistItem
{
    [JsonPropertyName("track")]
    public SpotifyTrack? Track { get; set; }
}