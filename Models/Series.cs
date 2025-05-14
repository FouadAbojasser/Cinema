namespace Cinema.Models
{
    public class Series
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string Trailer { get; set; } = string.Empty;
        public DateOnly ProductionDate { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string Duration { get; set; } = string.Empty;
        public double Rate { get; set; }
        public int Season { get; set; }
        public int Episodes { get; set; }
        public ICollection<Genre> Genres { get; set; } = [];
        public ICollection<Image> Images { get; } =  [];
        public ICollection<Actor> Actors { get; set; } = [];
        public int? DirectorId { get; set; } 
        public Director? Director { get; set; } = null!;

    }

}
