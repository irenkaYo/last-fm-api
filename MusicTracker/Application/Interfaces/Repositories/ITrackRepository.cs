using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface ITrackRepository
{
    public Task<List<Track>> GetAllTracks();
    public Task SaveTracks(List<Track> tracks);
    public Task<List<Track>> GetUserTracks(List<ListeningHistory> history);
    public Task<List<Track>> GetTracksByIds(List<Guid> trackIds);
}