namespace Cinema.Models
{
    public class MovieRates
    {
        public int Id { get; set; }
        public string ApplicationUserName { get; set; } = null!;
        public int MovieId { get; set; }
        public int? ActorsRate { get; set; }
        public int? GraphicsRate { get; set; }
        public int? DirectorRate { get; set; }
        public int? OverallRate { get; set; }
    }
}
