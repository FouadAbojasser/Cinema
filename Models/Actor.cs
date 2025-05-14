using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        public DateOnly DoB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Img { get; set; } = string.Empty;
        public int? NoOfMovies { get; set; }
        public ICollection<Movie> Movies { get; set; } = [];
        public ICollection<Series> Series { get; set; } = [];
    }
}
