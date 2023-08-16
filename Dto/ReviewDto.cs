using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        [Precision(3, 2)]
        public decimal Rating { get; set; }
        //public Reviewer Reviewer { get; set; }
        //public MovieOnDvd Movie { get; set; }
    }
}
