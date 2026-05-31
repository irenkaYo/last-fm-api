namespace Application.DTOs.External.LastFm.Tracks;

public class TrackStatisticDto
{
    public int TrackCount { get; set; }
    public int ArtistCount { get; set; }
    public int GeneralDuration { get; set; }

    public TrackStatisticDto(int trackCount, int artistCount, int generalDuration)
    {
        TrackCount = trackCount;
        ArtistCount = artistCount;
        GeneralDuration = generalDuration;
    }
}