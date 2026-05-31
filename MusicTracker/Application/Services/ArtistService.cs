using Application.DTOs.External;
using Application.Interfaces;

namespace Application.Services;

public class ArtistService : IArtistService
{
    private readonly IMusicApiClient _client;
    
    public ArtistService(IMusicApiClient client)
    {
        _client = client;
    }

    public async Task<List<ArtistDto>> GetTopArtists(string userName)
    {
       
    }
}