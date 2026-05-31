using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<DateTime?> GetLastPlayedAt(string userName)
    {
        return await _dbContext.ListeningHistories
            .Where(h => h.UserName == userName)
            .MaxAsync(h => (DateTime?)h.PlayedAt);
    }

    public async Task<List<ListeningHistory>> GetHistoryByUserName(string userName)
    {
        return await _dbContext.ListeningHistories.Where(h => h.UserName == userName).ToListAsync();
    }
    
    public async Task<List<ListeningHistory>> GetHistoryByUserNameAndTime(string userName, DateTime startDate, DateTime endDate)
    {
        return await _dbContext.ListeningHistories.Where(h => h.UserName == userName && h.PlayedAt >= startDate && h.PlayedAt <= endDate).ToListAsync();
    }
}