using System.Text;

using Microsoft.AspNetCore.Components;

using TheTruthHurtsMe.Services;

namespace TheTruthHurtsMe.Components.Pages;

public partial class SpotifyPlaylist : ComponentBase
{
    [Inject]
    private SpotifyService SpotifyService { get; set; } = default!;
        
    [Inject] 
    private GPTService GPTService { get; set; } = default!;
    
    private string? _spotifyUrl;
    private string? _analysis;

    private string? _error;
    private string? _status;

    private bool _analysisInProgress;

    private string _systemMessage = "I am going to give you a bunch of songs in the format {{artist:song_name}}. Create an analysis on my personality based off these songs/artists and create a summary of at least two paragraphs. Make sure to write [NEWLINE] when you are starting a new paragraph. Here is my playlist: ";

    private async Task AnalyzeAsync()
    {
        if (_analysisInProgress)
        {
            return;
        }
        
        _analysisInProgress = true;
        
        _error = "";
        _status = "";
        _analysis = "";

        try
        {
            if (string.IsNullOrWhiteSpace(_spotifyUrl))
            {
                _error = "Please enter Spotify playlist url.";
                return;
            }

            var spotifyPart = _spotifyUrl.Split("playlist/");
            if (spotifyPart.Length < 2)
            {
                _error = "Invalid spotify playlist url.";
                return;
            }

            _status = "Fetching tracks..";

            var playlistId = spotifyPart[1].Split('?')[0];

            var playlist = await SpotifyService.GetSpotifyPlaylistAsync(playlistId);
            if (playlist is null ||
                playlist.Tracks is null ||
                playlist.Tracks.Items is null)
            {
                _error = "Failed to get playlist.";
                return;
            }

            var sb = new StringBuilder();

            var maxTracks = 100;
            var tracks = playlist.Tracks.Items;
            foreach (var track in tracks.Take(maxTracks))
            {
                if (track.Track is null)
                {
                    continue;
                }

                string? trackName = track.Track.Name;
                if (trackName is null)
                {
                    continue;
                }

                string? artistName = null;

                if (track.Track.Artists is not null &&
                    track.Track.Artists.Count > 0)
                {
                    artistName = track.Track?.Artists[0].Name;
                }

                sb.Append($"{{{artistName ?? "Unknown Artist"}:{trackName}}}");
            }

            _status = "Analyzing playlist..";
            StateHasChanged();

            await Task.Delay(2000);

            var response = await GPTService.PromptAsync(sb.ToString(), _systemMessage);

            _status = "";
            await TypeResponseAsync(response.Replace("[NEWLINE]", "\n").Trim());
        }
        catch (Exception)
        {
            _status = "";
            _error = "Failed to process/analyze Spotify playlist.";
        }
        finally
        {
            _analysisInProgress = false;
        }
    }

    private async Task TypeResponseAsync(string response)
    {
        int msPerCharacter = 20;
        foreach (var character in response)
        {
            _analysis += character;
            StateHasChanged();
            await Task.Delay(msPerCharacter);
        }
        
        _analysisInProgress = false;
    }
}