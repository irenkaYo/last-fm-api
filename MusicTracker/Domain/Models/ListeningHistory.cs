namespace Domain.Models;

public class ListeningHistory
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public string UserName { get; set; }
    public DateTime PlayedAt { get; set; }
}