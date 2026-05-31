using Application.DTOs.External;
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
    
       
    }

    public async Task<List<RecentTrackDto>> GetUserRecentTracks(string userName)
    {
        var response = await _client.GetUserRecentTracks(userName);
        
        if (response ==  null)
            throw new Exception("No recent tracks found");

        var recentTracks = response.RecentTracks.Track;
        
        //перед сохранением в бд, я должна проверить, если ли там такой трек, чтоб не перепроверять duration
        
        return recentTracks;
    }
}