using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class MovieReviews
    {
        public int Id { get; set; }
        public string ApplicationUserName { get; set; } = null!;
        public int MovieId { get; set; }
        [MaxLength(50)]
        public string? Comment { get; set; }=string.Empty;
        public DateTime? CreatedAt { get; set; } 
        public Movie? Movie { get; set; }

    }
}
