using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Dtos.Song;
using MusicService.Services;
using MusicService.Services.Song;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<List<GetSongDto>>>> GetAllSongs()
        {
            var response = await _songService.GetAllSongs();

            if (response.Data == null)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("{songId}")]
        public async Task<ActionResult<ServiceResponse<GetSongDto>>> GetSong(int songId)
        {
            var response = await _songService.GetSong(songId);

            if (response.Data == null)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetSongDto>>> AddSong(AddSongDto request)
        {
            var response = await _songService.AddSong(request);

            if (!response.Success)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, response);

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetSongDto>>> UpdateSong(UpdateSongDto request)
        {
            var response = await _songService.UpdateSong(request);

            if (response.Data == null)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("{songId}")]
        public async Task<ActionResult<ServiceResponse<GetSongDto>>> DeleteSong(int songId)
        {
            var response = await _songService.DeleteSong(songId);

            if (!response.Success)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, response);

            return StatusCode(StatusCodes.Status204NoContent, response);
        }
    }
}
