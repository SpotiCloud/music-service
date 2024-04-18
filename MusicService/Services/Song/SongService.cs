using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Dtos.Song;
using MusicService.Models;

namespace MusicService.Services.Song
{
    public class SongService : ISongService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public SongService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetSongDto>>> GetAllSongs()
        {
            ServiceResponse<List<GetSongDto>> response = new ServiceResponse<List<GetSongDto>>();
            try
            {
                List<Models.Song> songs = await _context.song.ToListAsync();

                if (songs.Count > 0)
                {
                    response.Data = songs.Select(s => _mapper.Map<Models.Song, GetSongDto>(s)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Nothing was found!";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetSongDto>> GetSong(int songId)
        {
            ServiceResponse<GetSongDto>? response = new ServiceResponse<GetSongDto>();
            try
            {
                Models.Song? song = await _context.song
                    .Where(b => b.Id == songId)
                    .FirstAsync();

                if (song != null)
                {
                    response.Data = _mapper.Map<Models.Song, GetSongDto>(song);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Nothing was found!";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetSongDto>> AddSong(AddSongDto request)
        {
            ServiceResponse<GetSongDto> response = new ServiceResponse<GetSongDto>();

            try
            {
                Models.Song song = _mapper.Map<Models.Song>(request);
                _context.song.Add(song);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetSongDto>(song);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetSongDto>> UpdateSong(UpdateSongDto request)
        {
            ServiceResponse<GetSongDto> response = new ServiceResponse<GetSongDto>();

            try
            {
                Models.Song? song = _context.Find<Models.Song>(request.Id);
                song.Name = request.Name;
                song.Description = request.Description;
                song.Adress = request.Adress;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetSongDto>(song);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public Task<ServiceResponse<GetSongDto>> DeleteSong(int songId)
        {
            throw new NotImplementedException();
        }
    }
}