using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("artists")]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTopArtists([FromQuery] string userName)
    {
        var artists = await _artistService.GetTopArtists(userName);
        return Ok(artists);
    }
}