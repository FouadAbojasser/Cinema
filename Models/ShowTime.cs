namespace Cinema.Models
{
    public class ShowTime
    {
        public int Id { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeOnly ShowTimeFrom { get; set; }
        public TimeOnly ShowTimeTo { get; set; }
        //Foreign Key
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        //Navigation Property
        public MovieTheater MovieTheater { get; set; } = null!;
    }
}
