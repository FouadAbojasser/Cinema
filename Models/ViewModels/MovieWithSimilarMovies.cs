namespace Cinema.Models.ViewModels
{
    public class MovieWithSimilarMovies
    {
        public Movie? Movie {  get; set; }
        public List<Movie> SimilarMovies { get; set; } = [];

    }
}
