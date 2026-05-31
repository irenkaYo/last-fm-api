namespace Application.DTOs.External.LastFm.Tracks;

public class TrackStatisticDto
{
    public int TrackCount { get; set; }
    public int ArtistCount { get; set; }
    public TimeSpan GeneralDuration { get; set; }

    public TrackStatisticDto(int trackCount, int artistCount, TimeSpan generalDuration)
    {
        TrackCount = trackCount;
        ArtistCount = artistCount;
        GeneralDuration = generalDuration;
    }
}