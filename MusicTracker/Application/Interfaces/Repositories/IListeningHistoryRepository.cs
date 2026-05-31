using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IListeningHistoryRepository
{
    public Task SaveHistory(List<ListeningHistory> history);
    public Task<DateTime?> GetLastPlayedAt(string userName);
    public Task<List<ListeningHistory>> GetHistoryByUserName(string userName);
    public Task<List<ListeningHistory>> GetHistoryByUserNameAndTime(string userName, DateTime startDate, DateTime endDate);
}