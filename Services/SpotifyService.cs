using System.Text.Json;

using SpotifyAPI.Web;

using TheTruthHurtsMe.Services.Models;

namespace TheTruthHurtsMe.Services;

public class SpotifyService
{
    private readonly HttpClient _httpClient;
    
    public SpotifyService()
    {
        _httpClient = new HttpClient();
    }

    public async Task InitializeAsync(string clientId, string clientSecret)
    {
        var accessToken = await GetSpotifyAccessToken(clientId, clientSecret);
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
    }

    private async Task<string> GetSpotifyAccessToken(string clientId, string clientSecret)
    {
        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(clientId, clientSecret);
        var response = await new OAuthClient(config).RequestToken(request);

        return response.AccessToken;
    }

    public async Task<SpotifyPlaylistResponse?> GetSpotifyPlaylistAsync(string playlistId)
    {
        var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlistId}");
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed playlist lookup with error: {content}");
        }

        var jsonStream = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<SpotifyPlaylistResponse>(jsonStream);
    }
}