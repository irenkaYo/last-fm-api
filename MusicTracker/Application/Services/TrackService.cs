using Application.DTOs.External;
using Application.DTOs.External.RecentTracks;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Models;

namespace Application.Services;

public class TrackService : ITrackService
{
    private readonly IMusicApiClient _client;
    private readonly ITrackRepository _trackRepository;
    
    public TrackService(IMusicApiClient client, ITrackRepository trackRepository)
    {
        _client = client;
        _trackRepository = trackRepository;
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

        List<Track> allTracks = await _trackRepository.GetAllTracks();
        
        //перед сохранением в бд, я должна проверить, если ли там такой трек, чтоб не перепроверять duration
        //не сохранять уже существующие прослушивания, нужна проверка
        
        return recentTracks;
    }
}