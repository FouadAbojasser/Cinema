namespace Cinema.Models
{
    public class Director 
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly DoB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Img { get; set; } = string.Empty;
        public int? NoOfMovies { get; set; }
        public ICollection<Movie> Movies { get; set; } = [];
        public ICollection<Series> Series { get; set; } = [];
    }
}
