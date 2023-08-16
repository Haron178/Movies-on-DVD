using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IMovieRepository
    {
        ICollection<MovieOnDvd> GetMovies();
        MovieOnDvd GetMovieById(int id);
        MovieOnDvd GetMovieByName(string name);
        bool MovieExist(int movieId);
        bool MovieExist(string movieName);
        decimal GetMovieRating(int movieId);
        bool CreateMovie(ICollection<int> genreId, MovieOnDvd movie);
        bool Save();
        bool DeleteMovie(MovieOnDvd movie);
        bool UpdateMovie(MovieOnDvd movie);
        ICollection<MovieOnDvd> GetMoviesByGenre(int genreId);
        bool DeleteMovies(ICollection<MovieOnDvd> movies);
        ICollection<MovieOnDvd> GetMoviesByIds(int[] ids);
        ICollection<MovieOnDvd> GetMoviesByOwnerId(int ownerId);
    }
}
