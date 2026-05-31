using Domain.Models;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class MusicTrackerDbContext : DbContext
{
    public DbSet<Track> Tracks { get; set; }
    public DbSet<ListeningHistory> ListeningHistories { get; set; }
    
    public MusicTrackerDbContext(DbContextOptions<MusicTrackerDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ListeningHistoryConfiguration());
    }
}