namespace Cinema.Models.ViewModels
{
    public class TheatersWithScheduleVM
    {
        public Theater Theater { get; set; } = null!;
        public List<TheaterSchedule> TheaterSchedules { get; set; } = [];
        public List<Theater> ListOfTheater { get; set; } = [];
    }
}
