using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class MovieReviews
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string ApplicationUserName { get; set; } = null!;
        public int MovieId { get; set; }
        public string? Comment { get; set; }=string.Empty;
        public DateTime? CreatedAt { get; set; } 
        public Movie? Movie { get; set; }

    }
}
