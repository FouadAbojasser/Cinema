namespace Cinema.Models.ViewModels
{
    public class MovieGenresActorsDirectorsVM
    {
        public Movie movie {  get; set; }=new Movie();
        public List<Genre> Genres { get; set; } = [];
        public List<Actor> Actors { get; set; } = [];
        public List<Director> Directors { get; set; } = [];

    }
}
