namespace Cinema.Models.ViewModels
{
    public class MovieWithSimilarMoviesWithReviewsVM
    {
        public Movie? Movie {  get; set; }
        public List<Movie> SimilarMovies { get; set; } = [];
        public List<Review>? Reviews { get; set; } = [];

    }
}
