using Application.DTOs.External.RecentTracks;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Models;

namespace Application.Services;

public class SyncService : ISyncService
{
    private readonly IMusicApiClient _client;
    private readonly ITrackRepository _trackRepository;
    private readonly IListeningHistoryRepository _listeningHistoryRepository;
    
    public SyncService(IMusicApiClient client, ITrackRepository trackRepository, IListeningHistoryRepository listeningHistoryRepository)
    {
        _client = client;
        _trackRepository = trackRepository;
        _listeningHistoryRepository = listeningHistoryRepository;
    }
    
    public async Task SaveRecentTracks(string userName)
    {
        var response = await _client.GetUserRecentTracks(userName);

        if (response == null)
            throw new Exception("No recent tracks found");

        var recentTracks = response.RecentTracks.Track;
        
        var scrobbledTracks = recentTracks
            .Where(t => t.Date != null)
            .ToList();

        var allTracks = await SaveNewTracks(scrobbledTracks);

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
            .DistinctBy(rt => new
            {
                rt.Name,
                Artist = rt.Artist.Name
            })
            .ToList();

        List<Track> tracksToSave = [];

        foreach (var newTrack in newTracks)
        {
            var answer = await _client.GetTrackInfo(
                newTrack.Name,
                newTrack.Artist.Name);

            TimeSpan totalDuration = new TimeSpan();
            if (answer?.Track.Duration != null)
            {
                int duration = int.Parse(answer.Track.Duration);
                totalDuration = TimeSpan.FromMilliseconds(duration);
            }
            else
            {
                totalDuration = TimeSpan.Zero;
            }

            tracksToSave.Add(
                new Track(
                    newTrack.Name,
                    totalDuration,
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
            if (recent.Date == null)
                continue;
            
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
}