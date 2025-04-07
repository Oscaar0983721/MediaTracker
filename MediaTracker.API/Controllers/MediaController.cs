using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediaTracker.API.Models;
using MediaTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.API.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("movies")]
        public async Task<ActionResult<List<Media>>> GetMovies()
        {
            try
            {
                var movies = await _mediaService.GetAllMoviesAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("series")]
        public async Task<ActionResult<List<Media>>> GetSeries()
        {
            try
            {
                var series = await _mediaService.GetAllSeriesAsync();
                return Ok(series);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Media>> GetById(int id)
        {
            try
            {
                var media = await _mediaService.GetByIdAsync(id);
                if (media == null)
                {
                    return NotFound(new { error = "Media not found" });
                }
                return Ok(media);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<List<Media>>> GetByGenre(string genre)
        {
            try
            {
                var mediaList = await _mediaService.GetByGenreAsync(genre);
                return Ok(mediaList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Media>>> Search([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(new { error = "Search query is required" });
                }

                var results = await _mediaService.SearchAsync(query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<List<Comment>>> GetComments(int id)
        {
            try
            {
                var comments = await _mediaService.GetCommentsAsync(id);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult<Comment>> AddComment(int id, [FromBody] CommentRequest request)
        {
            try
            {
                // In a real app, we would get the user ID from the authenticated user
                // For this example, we'll use a mock user ID
                int userId = 1;

                var comment = await _mediaService.AddCommentAsync(id, userId, request.Content);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class CommentRequest
    {
        public string Content { get; set; }
    }
}