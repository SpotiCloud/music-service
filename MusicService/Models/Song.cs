using System.ComponentModel.DataAnnotations;

namespace MusicService.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Adress { get; set; } = string.Empty;
    }
}
