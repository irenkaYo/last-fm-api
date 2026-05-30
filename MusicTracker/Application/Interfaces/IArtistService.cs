using Application.DTOs.External;

namespace Application.Interfaces;

public interface IArtistService
{
    public Task<List<ArtistDto>> GetTopArtists(string userName);
}