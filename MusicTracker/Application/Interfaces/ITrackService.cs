using Application.DTOs.External;
using Application.DTOs.External.RecentTracks;

namespace Application.Interfaces;

public interface ITrackService
{
    public Task<List<TrackDto>> GetTopTracks(string userName);
    public Task<List<RecentTrackDto>> GetUserRecentTracks(string userName);
}