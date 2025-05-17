namespace Cinema.Models.ViewModels
{
    public class AddReviewVM
    {
        public int MovieId { get; set; }
        public string? Comment { get; set; }
        public int? UserRate { get; set; }
    }
}
