using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        [Precision(3,2)]
        public decimal Rating { get; set; }
        public Reviewer Reviewer { get; set; }
        public MovieOnDvd Movie { get; set;}
    }
}
