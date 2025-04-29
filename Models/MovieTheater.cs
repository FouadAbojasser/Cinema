namespace Cinema.Models
{
    public class MovieTheater
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public int TheaterId { get; set; }
        public Theater Theater { get; set; } = null!;
        public int? TotalNumberOfTickets { get; set; }
        public int? ReservedTickets { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

    }
}
