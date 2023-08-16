using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReviewById(int id);
        Review GetReviewByTitle(string title);
        bool ReviewExist(int id);
        bool ReviewExist(string name);
        ICollection<Review> GetReviewsByReviewerId(int ReviewId);
        bool Save();
        bool AddReview(int movieId,int ReviewerId, Review newReview);
        ICollection<Review> GetReviewsByMovieId(int movieId);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(ICollection<Review> reviews);
    }
}
