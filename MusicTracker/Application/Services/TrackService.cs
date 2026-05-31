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
    private readonly IMusicApiClient _client;
    private readonly ITrackRepository _trackRepository;
    private readonly IListeningHistoryRepository _listeningHistoryRepository;
    
    public TrackService(IMusicApiClient client, ITrackRepository trackRepository, IListeningHistoryRepository listeningHistoryRepository)
    {
        _client = client;
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

        int generalDuration = userTracks.Sum(t => t.Duration);

        TrackStatisticDto statistic = new TrackStatisticDto(trackCount, artistCount, generalDuration);
        return statistic;
    }

    public async Task SaveRecentTracks(string userName)
    {
        var response = await _client.GetUserRecentTracks(userName);

        if (response == null)
            throw new Exception("No recent tracks found");

        var recentTracks = response.RecentTracks.Track;

        var allTracks = await SaveNewTracks(recentTracks);

        var history = await CreateListeningHistory(
            userName,
            recentTracks,
            allTracks);

        await _listeningHistoryRepository.SaveHistory(history);
    }
    
    private async Task<List<Track>> SaveNewTracks(List<RecentTrackDto> recentTracks)
    {
        var allTracks = await _trackRepository.GetAllTracks();

        var newTracks = recentTracks
            .Where(rt => !allTracks.Any(t =>
                t.Name == rt.Name &&
                t.ArtistName == rt.Artist.Name))
            .ToList();

        List<Track> tracksToSave = [];

        foreach (var newTrack in newTracks)
        {
            var answer = await _client.GetTrackInfo(
                newTrack.Name,
                newTrack.Artist.Name);

            int duration = int.Parse(answer.Track.Duration);

            tracksToSave.Add(
                new Track(
                    newTrack.Name,
                    duration,
                    newTrack.Artist.Name));
        }

        await _trackRepository.SaveTracks(tracksToSave);

        allTracks.AddRange(tracksToSave);

        return allTracks;
    }
    
    private async Task<List<ListeningHistory>> CreateListeningHistory(string userName, List<RecentTrackDto> recentTracks, List<Track> allTracks)
    {
        DateTime? lastPlayedAt =
            await _listeningHistoryRepository.GetLastPlayedAt(userName);

        List<ListeningHistory> history = [];

        foreach (var recent in recentTracks)
        {
            DateTime playedAt = DateTimeOffset
                .FromUnixTimeSeconds(long.Parse(recent.Date.Uts))
                .UtcDateTime;

            if (lastPlayedAt != null && playedAt <= lastPlayedAt)
                continue;

            var trackId = allTracks.First(t =>
                t.Name == recent.Name &&
                t.ArtistName == recent.Artist.Name).Id;

            history.Add(
                new ListeningHistory(
                    trackId,
                    userName,
                    playedAt));
        }

        return history;
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