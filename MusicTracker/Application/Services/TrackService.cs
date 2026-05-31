using Application.DTOs.External;
using Application.DTOs.External.LastFm.Tracks;
using Application.DTOs.External.RecentTracks;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Models;

namespace Application.Services;

public class TrackService : ITrackService
{
    private static readonly int TopAtistsCount = 5;
    private static readonly int TopTracksCount = 10;
    private readonly ITrackRepository _trackRepository;
    private readonly IListeningHistoryRepository _listeningHistoryRepository;
    
    public TrackService(ITrackRepository trackRepository, IListeningHistoryRepository listeningHistoryRepository)
    {
        _trackRepository = trackRepository;
        _listeningHistoryRepository = listeningHistoryRepository;
    }

    public async Task<List<string>> GetTopArtists(string userName)
    {
        List<Track> userTracks = await GetUserTracks(userName);

        return userTracks
            .GroupBy(t => t.ArtistName)
            .OrderByDescending(g => g.Count())
            .Take(TopAtistsCount)
            .Select(g => g.Key)
            .ToList();
    }

    public async Task<List<Track>> GetTopTracks(string userName)
    {
        List<Track> userTracks = await GetUserTracks(userName);

        return userTracks
            .GroupBy(t => t.Id)
            .OrderByDescending(g => g.Count())
            .Take(TopTracksCount)
            .Select(g => g.First())
            .ToList();
    }

    public async Task<TrackStatisticDto> GetUserStatistic(string userName, DateTime startDate, DateTime endDate)
    {
        List<Track> userTracks = await GetUserTracksByTime(userName, startDate, endDate);

        int trackCount = userTracks.Count;

        int artistCount = userTracks
            .Select(t => t.ArtistName)
            .Distinct()
            .Count();

        TimeSpan totalDuration = TimeSpan.FromTicks(userTracks.Sum(t => t.Duration.Ticks) );
        
        TrackStatisticDto statistic = new TrackStatisticDto(trackCount, artistCount, totalDuration);
        return statistic;
    }
    
    private async Task<List<Track>> GetUserTracks(string userName)
    {
        var userHistory = await  _listeningHistoryRepository.GetHistoryByUserName(userName);
        List<Track> userTracks = await _trackRepository.GetUserTracks(userHistory);
        return userTracks;
    }
    
    private async Task<List<Track>> GetUserTracksByTime(string userName, DateTime startDate, DateTime endDate)
    {
        var userHistory = await  _listeningHistoryRepository.GetHistoryByUserNameAndTime(userName, startDate, endDate);
        List<Track> userTracks = await _trackRepository.GetUserTracks(userHistory);
        return userTracks;
    }
}