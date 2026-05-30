using Application.DTOs.External;

namespace Application.Interfaces;

public interface ITrackService
{
    public Task<List<TrackDto>> GetTopTracks(string userName);
}