using Application.Interfaces.Repositories;
using Domain.Models;

namespace Infrastructure.Persistence.Repositories;

public class ListeningHistoryRepository : IListeningHistoryRepository
{
    private readonly MusicTrackerDbContext _dbContext;

    public ListeningHistoryRepository(MusicTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveHistory(List<ListeningHistory> history)
    {
        await _dbContext.ListeningHistories.AddRangeAsync(history);
        await _dbContext.SaveChangesAsync();
    }
    
}