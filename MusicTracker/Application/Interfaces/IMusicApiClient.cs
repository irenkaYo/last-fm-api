using Application.DTOs.External;
using Application.DTOs.External.RecentTracks;

namespace Application.Interfaces;

public interface IMusicApiClient
{
    public Task<RecentTracksResponseDto?> GetUserRecentTracks(string userName);
}