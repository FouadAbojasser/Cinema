namespace Cinema.Models.ViewModels
{
    public class CinemaMovieShowTimesVM
    {
        public Movie? movie { get; set; }
        public List<TheaterSchedule>? TheaterSchedules { get; set; }

    }
}
