using MusicService.Dtos.Song;

namespace MusicService.Services.Song
{
    public interface ISongService
    {
        public Task<ServiceResponse<List<GetSongDto>>> GetAllSongs();
        public Task<ServiceResponse<GetSongDto>> GetSong(int songId);
        public Task<ServiceResponse<GetSongDto>> AddSong(AddSongDto request);
        public Task<ServiceResponse<GetSongDto>> UpdateSong(UpdateSongDto request);
        public Task<ServiceResponse<GetSongDto>> DeleteSong(int songId);
    }
}
