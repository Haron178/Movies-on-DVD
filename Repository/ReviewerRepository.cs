using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewerById(int id)
        {
            return _context.Reviewers.FirstOrDefault(r => r.Id == id);
        }

        public Reviewer GetReviewerByName(string name)
        {
            return _context.Reviewers.FirstOrDefault(r => r.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.OrderBy(r => r.Id).ToList();
        }

        public bool ReviewerExist(string name)
        {
            return _context.Reviewers.Any(r => r.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public bool ReviewerExist(int id)
        {
            return _context.Reviewers.Any(r => r.Id == id);
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}
