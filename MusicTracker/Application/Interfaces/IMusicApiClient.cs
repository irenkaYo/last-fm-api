using Application.DTOs.External;
using Application.DTOs.External.Info;
using Application.DTOs.External.RecentTracks;

namespace Application.Interfaces;

public interface IMusicApiClient
{
    public Task<RecentTracksResponseDto?> GetUserRecentTracks(string userName);
    public Task<TrackInfoResponseDto?> GetTrackInfo(string trackName, string artistName);
}