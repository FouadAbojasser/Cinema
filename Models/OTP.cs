namespace Cinema.Models
{
    public class OTP
    {
        public int Id { get; set; }
        public int OTP_Number {  get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime RequestDateTime { get; set; }
        public DateTime ExpairationDateTime { get; set; }
        public ApplicationUser applicationUser { get; set; } = null!;
        public bool UsedByUser { get; set; }

    }
}
