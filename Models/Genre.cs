namespace Cinema.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberOfMovies { get; set; }
        public string? Img { get; set; } = string.Empty ;
        public ICollection<Movie> Movies { get; set; } = [];
        public ICollection<Series> Series { get; set; } = [];
    }

}
