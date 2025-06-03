namespace Cinema.Models
{
    public class TheaterSchedule
    {
        public int Id { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeOnly ShowTimeFrom { get; set; }
        public TimeOnly ShowTimeTo { get; set; }
        //Foreign Key
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public Movie? Movie { get; set; }
        public Theater? Theater { get; set; }
    }
}
