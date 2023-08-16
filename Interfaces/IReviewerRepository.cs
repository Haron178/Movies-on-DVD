using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewerById(int id);
        Reviewer GetReviewerByName(string name);
        bool ReviewerExist(string name);
        bool ReviewerExist(int id);
        bool Save();
        bool AddReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);

    }
}
