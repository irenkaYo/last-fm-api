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
}