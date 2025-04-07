using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaTracker.API.Models;
using MediaTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}/watchlist")]
        public async Task<ActionResult<List<Media>>> GetWatchlist(int id)
        {
            try
            {
                var watchlist = await _userService.GetWatchlistAsync(id);
                return Ok(watchlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("{id}/watchlist")]
        public async Task<ActionResult<WatchlistItem>> AddToWatchlist(int id, [FromBody] WatchlistRequest request)
        {
            try
            {
                var watchlistItem = await _userService.AddToWatchlistAsync(id, request.MediaId, request.MediaType);
                return Ok(watchlistItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}/watchlist/{mediaId}")]
        public async Task<ActionResult> RemoveFromWatchlist(int id, int mediaId)
        {
            try
            {
                var success = await _userService.RemoveFromWatchlistAsync(id, mediaId);
                if (!success)
                {
                    return NotFound(new { error = "Watchlist item not found" });
                }
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class WatchlistRequest
    {
        public int MediaId { get; set; }
        public string MediaType { get; set; }
    }
}