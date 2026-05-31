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
    public async Task<IActionResult> GetTopTracks([FromQuery] string userName)
    {
        var tracks = await _trackService.GetTopTracks(userName);
        return Ok(tracks);
    }
    
    [HttpGet("top/artists")]
    public async Task<IActionResult> GetTopArtists([FromQuery] string userName)
    {
        var artists = await _trackService.GetTopArtists(userName);
        return Ok(artists);
    }

    [HttpGet("statistic")]
    public async Task<IActionResult> GetStatistic([FromQuery] string userName, [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var statistic = await _trackService.GetUserStatistic(userName, from, to);
        return Ok(statistic);
    }
}