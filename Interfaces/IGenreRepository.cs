using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IGenreRepository
    {
        ICollection<Genre> GetGenres();
        Genre GetGenreById(int id);
        Genre GetGenreByName(string name);
        bool GenreExist(string name);
        bool GenreExist(int id);
        bool AddGenre(Genre genre);
        bool Save();
        bool UpdateGenre(Genre genre);
        bool DeleteGenre(Genre genre);
        ICollection<Genre> GetGenresByMovieId(int movieId);
    }
}
