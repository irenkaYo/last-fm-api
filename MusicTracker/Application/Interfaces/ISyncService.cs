namespace Application.Interfaces;

public interface ISyncService
{
    public Task SaveRecentTracks(string userName);
}