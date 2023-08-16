using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddReview(int movieId ,int reviewerId, Review newReview)
        {
            var reviewer = _context.Reviewers.FirstOrDefault(r => r.Id == reviewerId);
            var movie = _context.MoviesOnDvd.FirstOrDefault(r => r.Id == movieId);
            if(reviewer == null || movie == null)
                return false;
            newReview.Reviewer = reviewer;
            if(reviewer.Reviews == null)
                reviewer.Reviews = new List<Review>();
            newReview.Movie = movie;
            if(movie.Reviews == null)
                movie.Reviews = new List<Review>();
            movie.Reviews.Add(newReview);
            reviewer.Reviews.Add(newReview);
            _context.Reviews.Add(newReview);
            return Save();
        }

        public ICollection<Review> GetReviewsByReviewerId(int reviewId)
        {
            return _context.Reviews.Where(r => r.Reviewer.Id == reviewId).ToList();
        }

        public Review GetReviewById(int id)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id ==id);
        }

        public ICollection<Review> GetReviewsByMovieName(string movieName)
        {
            return _context.Reviews.Where(r => r.Movie.Name.Trim().ToUpper() == movieName.Trim().ToUpper()).ToList();
        }

        public ICollection<Review> GetReviewsByMovieId(int movieId)
        {
            return _context.Reviews.Where(r => r.Movie.Id == movieId).ToList();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(r => r.Id).ToList();
        }

        public bool ReviewExist(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }

        public bool ReviewExist(string title)
        {
            return _context.Reviews.Any(r => r.Title.Trim().ToUpper() == title.Trim().ToUpper());
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public Review GetReviewByTitle(string title)
        {
            return _context.Reviews.FirstOrDefault(r => r.Title.Trim().ToUpper() == title.Trim().ToUpper());
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(ICollection<Review> reviews)
        {
            if (reviews == null)
                return true;
            _context.RemoveRange(reviews);
            return Save();
        }
    }
}
