using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddGenre(Genre genre)
        {
            _context.Add(genre);
            return Save();
        }

        public bool DeleteGenre(Genre genre)
        {
            _context.Remove(genre);
            return Save();
        }

        public bool GenreExist(string name)
        {
            return _context.Genres.Any(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }
        public bool GenreExist(int id)
        {
            return _context.Genres.Any(x => x.Id == id);
        }

        public Genre GetGenreById(int id)
        {
            return _context.Genres.FirstOrDefault(g => g.Id == id);
        }

        public Genre GetGenreByName(string name)
        {
            return _context.Genres.FirstOrDefault(g => g.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public ICollection<Genre> GetGenres()
        {
            return _context.Genres.OrderBy(x => x.Id).ToList();
        }

        public ICollection<Genre> GetGenresByMovieId(int movieId)
        {
            return _context.Genres.Where(g => g.Movies.Any(m => m.Id == movieId)).ToList();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateGenre(Genre genre)
        {
            _context.Update(genre);
            return Save();
        }
    }
}
