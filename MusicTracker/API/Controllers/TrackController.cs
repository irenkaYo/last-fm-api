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

    [HttpGet("top")]
    public async Task<IActionResult> GetTopTracks([FromQuery] string userName)
    {
        var tracks = await _trackService.GetTopTracks(userName);
        return Ok(tracks);
    }
}