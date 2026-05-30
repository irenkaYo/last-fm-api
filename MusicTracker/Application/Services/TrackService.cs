using Application.DTOs.External;
using Application.DTOs.External.Recent;
using Application.DTOs.External.RecentTracks;
using Application.Interfaces;

namespace Application.Services;

public class TrackService : ITrackService
{
    private readonly IMusicApiClient _client;
    
    public TrackService(IMusicApiClient client)
    {
        _client = client;
    }

    public async Task<List<TrackDto>> GetTopTracks(string userName)
    {
        var response = await _client.GetUserTopTracks(userName);

        if (response == null)
        {
            throw new Exception("No top tracks found");
        }

        var topTracks = response.TopTracks.Tracks;
        // dto in domain
        //сохранение в БД, вызов метода из репозитория
    
        return topTracks;
    }

    public async Task<List<RecentTrackDto>> GetUserRecentTracks(string userName)
    {
        var response = await _client.GetUserRecentTracks(userName);
        
        if (response ==  null)
            throw new Exception("No recent tarcks found");

        var recentTracks = response.RecentTracks.Track;
        
        return recentTracks;
    }
}