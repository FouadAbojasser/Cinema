namespace Cinema.Models.ViewModels
{
    public class MovieWithSimilarMoviesVM
    {
        public Movie? Movie {  get; set; }
        public List<Movie> SimilarMovies { get; set; } = [];

    }
}
