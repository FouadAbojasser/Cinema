namespace Cinema.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Comment { get; set; }=string.Empty;
        public int? UserRate { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public ApplicationUser? applicationUser { get; set; }
        public Movie? Movie { get; set; }

    }
}
