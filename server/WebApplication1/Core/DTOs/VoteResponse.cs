namespace WebApplication1.Core.DTOs
{
    public class VoteResponse
    {
        public int Votes { get; set; }
        public bool? IsCorrect { get; set; } // null לסקר
    }
}
