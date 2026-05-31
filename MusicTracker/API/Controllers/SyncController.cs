using Application.DTOs.External.RecentTracks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("sync")]
public class SyncController : ControllerBase
{
    private readonly ISyncService _syncService;

    public SyncController(ISyncService syncService)
    {
        _syncService = syncService;
    }

    [HttpPost("{userName}")]
    public async Task<IActionResult> Sync(string userName)
    {
        await _syncService.SaveRecentTracks(userName);

        return Ok();
    }
    
    
}