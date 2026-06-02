using System.Net.Http.Json;
using Application.DTOs.External.Info;
using Application.DTOs.External.RecentTracks;
using Application.Interfaces;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalServices.MusicApi;

public class MusicApiClient : IMusicApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    
    public MusicApiClient(HttpClient httpClient, IOptions<MusicApiSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
    }

    public async Task<RecentTracksResponseDto?> GetUserRecentTracks(string userName)
    {
        return await _httpClient.GetFromJsonAsync<RecentTracksResponseDto>($"2.0/?method=user.getrecenttracks&user={userName}&api_key={_apiKey}&format=json");
    }

    public async Task<TrackInfoResponseDto?> GetTrackInfo(string trackName, string artistName)
    {
        var artist = Uri.EscapeDataString(artistName);
        var track = Uri.EscapeDataString(trackName);
        
        return await _httpClient.GetFromJsonAsync<TrackInfoResponseDto>($"2.0/?method=track.getInfo&api_key={_apiKey}&artist={artist}&track={track}&format=json");
    }
}