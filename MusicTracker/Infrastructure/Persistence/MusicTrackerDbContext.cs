using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class MusicTrackerDbContext : DbContext
{
    public DbSet<Track> Tracks { get; set; }
    public DbSet<ListeningHistory> ListeningHistories { get; set; }
    
    public MusicTrackerDbContext() : base ()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
}