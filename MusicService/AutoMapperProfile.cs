using AutoMapper;
using MusicService.Dtos.Song;
using MusicService.Models;

namespace MusicService
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //All "Song" related mappings
            CreateMap<Song, GetSongDto>();
            CreateMap<AddSongDto, Song>();
            CreateMap<UpdateSongDto, Song>();
        }
    }
}
