using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("tracks")]
public class TrackController : ControllerBase
{
    private readonly ITrackService _trackService;

    public TrackController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet("top/tracks")]
    public async Task<IActionResult> GetTopTracks([FromQuery] string username)
    {
        var tracks = await _trackService.GetTopTracks(username);
        return Ok(tracks);
    }
    
    [HttpGet("top/artists")]
    public async Task<IActionResult> GetTopArtists([FromQuery] string username)
    {
        var artists = await _trackService.GetTopArtists(username);
        return Ok(artists);
    }

    [HttpGet("statistic")]
    public async Task<IActionResult> GetStatistic([FromQuery] string username, [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var statistic = await _trackService.GetUserStatistic(username, from, to);
        return Ok(statistic);
    }
}