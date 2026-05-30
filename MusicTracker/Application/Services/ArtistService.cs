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
        var response = await _client.GetUserTopArtists(userName);

        if (response == null)
        {
            throw new Exception("No top artist found");
        }
        
        var topArtists = response.TopArtists.Artists;
        
        //....
        
        return topArtists;
    }
}