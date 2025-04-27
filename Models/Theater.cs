namespace Cinema.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rate { get; set; }
        public int Seats { get; set; }
        public ICollection<Movie> Movies { get; set; }
       
    }

}
