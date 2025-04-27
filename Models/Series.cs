namespace Cinema.Models
{
    public class Series
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Trailer { get; set; }
        public DateOnly ProductionDate { get; set; }
        public string Duration { get; set; }
        public double Rate { get; set; }
        public int Season { get; set; }
        public int Episodes { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Image> Images { get; } = new List<Image>();
        public ICollection<Actor> Actors { get; set; }
        public int DirectorId { get; set; } // Required foreign key property
        public Director Director { get; set; } = null!; // Required reference navigation to principal

    }

}
