namespace MusicService.Dtos.Song
{
    public class GetSongDto
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
    }
}
