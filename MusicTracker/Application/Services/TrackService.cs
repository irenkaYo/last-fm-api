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
    private readonly IListeningHistoryRepository _listeningHistoryRepository;
    
    public TrackService(IMusicApiClient client, ITrackRepository trackRepository, IListeningHistoryRepository listeningHistoryRepository)
    {
        _client = client;
        _trackRepository = trackRepository;
        _listeningHistoryRepository = listeningHistoryRepository;
    }

    public async Task<List<TrackDto>> GetTopTracks(string userName)
    {
    
       
    }

    public async Task SaveRecentTracks(string userName)
    {
        var response = await _client.GetUserRecentTracks(userName);
        
        if (response ==  null)
            throw new Exception("No recent tracks found");

        var recentTracks = response.RecentTracks.Track;

        List<Track> allTracks = await _trackRepository.GetAllTracks();
        
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

            int trackDuration = int.Parse(answer.Track.Duration);

            tracksToSave.Add(
                new Track(
                    newTrack.Name,
                    trackDuration,
                    newTrack.Artist.Name));
        }

        await _trackRepository.SaveTracks(tracksToSave);
        
        DateTime? lastPlayedAt = await _listeningHistoryRepository.GetLastPlayedAt(userName);

        List<ListeningHistory> history = [];

        foreach (var recent in recentTracks)
        {
            DateTime playedAt = DateTimeOffset
                .FromUnixTimeSeconds(long.Parse(recent.Date.Uts))
                .UtcDateTime;
            
            var trackId = allTracks.First(t =>
                t.Name == recent.Name &&
                t.ArtistName == recent.Artist.Name).Id;
            
            if (lastPlayedAt == null || playedAt > lastPlayedAt)
                history.Add(new ListeningHistory(trackId, userName, playedAt));
        }
        
        await _listeningHistoryRepository.SaveHistory(history);
    }
}