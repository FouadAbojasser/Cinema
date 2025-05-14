namespace Cinema.Models.ViewModels
{
    public class MoviesWithFiltersVM
    {
        public List<Movie> Latest { get; set; } = [];
        public List<Movie> CommingSoon { get; set; } = [];
        public List<Movie> TopRated { get; set; } = [];
        public List<Movie> RecentlyReleased { get; set; } = [];

        public List<Movie> TopTrailerSection { get; set; } = [];
    }
}
