namespace Domain.Models;

public class ListeningHistory
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public string UserName { get; set; }
    public DateTime PlayedAt { get; set; }

    public ListeningHistory(Guid trackId, string userName, DateTime playedAt)
    {
        Id = Guid.NewGuid();
        TrackId = trackId;
        UserName = userName;
        PlayedAt = playedAt;
    }
}