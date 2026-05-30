using Application.DTOs.External;

namespace Application.Interfaces;

public interface IMusicApiClient
{
    public Task<TopTracksResponseDto?> GetUserTopTracks(string username);
}