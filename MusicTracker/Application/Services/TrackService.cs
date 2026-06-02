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
        var history = await _listeningHistoryRepository.GetHistoryByUserName(userName);

        var tracks = await _trackRepository.GetTracksByIds(
            history.Select(h => h.TrackId).Distinct());

        return history
            .GroupBy(h => tracks.First(t => t.Id == h.TrackId).ArtistName)
            .OrderByDescending(g => g.Count())
            .Take(TopAtistsCount)
            .Select(g => g.Key)
            .ToList();
    }

    public async Task<List<Track>> GetTopTracks(string userName)
    {
        var history = await _listeningHistoryRepository.GetHistoryByUserName(userName);

        var topTrackIds = history
            .GroupBy(h => h.TrackId)
            .OrderByDescending(g => g.Count())
            .Take(TopTracksCount)
            .Select(g => g.Key)
            .ToList();

        var tracks = await _trackRepository.GetTracksByIds(topTrackIds);

        return topTrackIds
            .Select(id => tracks.First(t => t.Id == id))
            .ToList();
    }

    public async Task<TrackStatisticDto> GetUserStatistic(string userName, DateTime startDate, DateTime endDate)
    {
        var history = await _listeningHistoryRepository
            .GetHistoryByUserNameAndTime(userName, startDate, endDate);

        var tracks = await _trackRepository.GetTracksByIds(
            history.Select(h => h.TrackId).Distinct());

        int trackCount = history.Count;

        int artistCount = tracks
            .Select(t => t.ArtistName)
            .Distinct()
            .Count();

        TimeSpan totalDuration = TimeSpan.FromTicks(
            history.Sum(h => tracks.First(t => t.Id == h.TrackId).Duration.Ticks));

        return new TrackStatisticDto(
            trackCount,
            artistCount,
            totalDuration);
    }
}