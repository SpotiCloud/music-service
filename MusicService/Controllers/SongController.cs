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

        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<List<GetSongDto>>>> GetAllSongs()
        {
            var response = await _songService.GetAllSongs();

            if (response.Data == null)
                return StatusCode(StatusCodes.Status404NotFound, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("{songId}")]
        public async Task<ActionResult<ServiceResponse<GetSongDto>>> GetSong(int songId)
        {
            var response = await _songService.GetSong(songId);

            if (response.Data == null)
                return StatusCode(StatusCodes.Status404NotFound, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetSongDto>>> AddSong(AddSongDto request)
        {
            var songResponse = await _songService.AddSong(request);

            if (!songResponse.Success)
                return StatusCode(StatusCodes.Status404NotFound, songResponse);

            return StatusCode(StatusCodes.Status201Created, songResponse);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetSongDto>>> UpdateSong(UpdateSongDto request)
        {
            var response = await _songService.UpdateSong(request);

            if (response.Data == null)
                return StatusCode(StatusCodes.Status404NotFound, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        /*[HttpDelete("{songId}")]
        public async Task<ActionResult<ServiceResponse<GetSongDto>>> DeleteSong(int songId)
        {
            /*var response = await _songService;

            if (response.Data == null)
                return StatusCode(StatusCodes.Status404NotFound, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }*/
    }
}
