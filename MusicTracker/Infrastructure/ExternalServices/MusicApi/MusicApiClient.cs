using System.Net;
using System.Net.Http.Json;
using Application.DTOs.External.Info;
using Application.DTOs.External.RecentTracks;
using Application.Exceptions;
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
        HttpResponseMessage response = await _httpClient.GetAsync(
            $"2.0/?method=user.getrecenttracks&user={userName}&api_key={_apiKey}&format=json");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException("User not found");
        }
        
        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalServiceException("Last.fm service is unavailable");
        }

        return await response.Content.ReadFromJsonAsync<RecentTracksResponseDto>();    
    }

    public async Task<TrackInfoResponseDto?> GetTrackInfo(string trackName, string artistName)
    {
        var artist = Uri.EscapeDataString(artistName);
        var track = Uri.EscapeDataString(trackName);

        HttpResponseMessage response = await _httpClient.GetAsync(
            $"2.0/?method=track.getInfo&api_key={_apiKey}&artist={artist}&track={track}&format=json");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException("User not found");
        }
        
        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalServiceException("Last.fm service is unavailable");
        }

        return await response.Content.ReadFromJsonAsync<TrackInfoResponseDto>();
    }
}