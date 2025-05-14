using System.Reflection.Metadata;

namespace Cinema.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int? MovieId { get; set; }
        public Movie Movie { get; set; } = new Movie();
        public int? SeriesId { get; set; } 
        public Series Series { get; set; } = new Series();

    }
}
