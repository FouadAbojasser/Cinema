namespace Cinema.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Rate { get; set; }
        public int Seats { get; set; }
        public ICollection<MovieTheater> MovieTheaters { get; set; } = [];

    }

}
