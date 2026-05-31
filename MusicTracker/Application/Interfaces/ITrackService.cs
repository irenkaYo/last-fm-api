using Application.DTOs.External;
using Application.DTOs.External.LastFm.Tracks;
using Application.DTOs.External.RecentTracks;
using Domain.Models;

namespace Application.Interfaces;

public interface ITrackService
{
    public Task<List<Track>> GetTopTracks(string userName);
    public Task<List<string>> GetTopArtists(string userName);
    public Task<TrackStatisticDto> GetUserStatistic(string userName, DateTime startDate, DateTime endDate);
}