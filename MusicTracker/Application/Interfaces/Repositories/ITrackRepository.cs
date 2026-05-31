using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface ITrackRepository
{
    public Task<List<Track>> GetAllTracks();
    public Task SaveTracks(List<Track> tracks);
}