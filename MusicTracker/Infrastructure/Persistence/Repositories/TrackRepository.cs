using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly MusicTrackerDbContext _dbContext;

    public TrackRepository(MusicTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Track>> GetAllTracks()
    {
        List<Track> tracks = await _dbContext.Tracks.ToListAsync();
        return tracks;
    }

    public async Task SaveTracks(List<Track> tracks)
    {
        await _dbContext.Tracks.AddRangeAsync(tracks);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<Track>> GetTracksByIds(IEnumerable<Guid> trackIds)
    {
        return await _dbContext.Tracks
            .Where(t => trackIds.Contains(t.Id))
            .ToListAsync();
    }
}