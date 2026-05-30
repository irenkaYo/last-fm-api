using System.Net.Http.Json;
using Application.DTOs.External;
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

    public async Task<TopTracksResponseDto?> GetUserTopTracks(string userName)
    {
        return await _httpClient.GetFromJsonAsync<TopTracksResponseDto>($"2.0/?method=user.gettoptracks&user={userName}&api_key={_apiKey}&format=json");
    }

    public async Task<TopArtistsResponseDto?> GetUserTopArtists(string userName)
    {
        return await _httpClient.GetFromJsonAsync<TopArtistsResponseDto>($"2.0/?method=user.gettopartists&user={userName}&api_key={_apiKey}&format=json");
    }
}