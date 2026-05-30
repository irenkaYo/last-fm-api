using Application.DTOs.External;
using Application.DTOs.External.RecentTracks;

namespace Application.Interfaces;

public interface IMusicApiClient
{
    public Task<TopTracksResponseDto?> GetUserTopTracks(string username);
    public Task<TopArtistsResponseDto?> GetUserTopArtists(string userName);
    public Task<RecentTracksResponseDto?> GetUserRecentTracks(string userName);
}