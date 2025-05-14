namespace Cinema.Models.ViewModels
{
    public class ActorWithMovies
    {
        public Actor? Actor { get; set; }
        public List<Movie>? ActorMovies { get; set; } 
    }
}
