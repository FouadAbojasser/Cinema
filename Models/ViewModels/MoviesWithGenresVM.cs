namespace Cinema.Models.ViewModels
{
    public class MoviesWithGenresVM
    {
        public List<Movie> Movies { get; set; } = [];
        public List<Genre> Genres { get; set; } = [];
        public List<Theater> ListOfTheater { get; set; } = [];
    }
}
