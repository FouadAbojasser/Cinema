namespace Cinema.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Rate { get; set; }
        public int Seats { get; set; }
        public ICollection<Movie> Movies { get; set; } = [];
        public ICollection<MovieTheater> MovieTheater { get; set; } = [];
    }

}
