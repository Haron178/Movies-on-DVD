using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;

        public MovieRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateMovie(ICollection<int> genreId, MovieOnDvd movie)
        {
            var genres = new List<Genre>();
            foreach (var id in genreId) 
            {
                genres.Add(_context.Genres.FirstOrDefault(g => g.Id == id));
            }
            if (genres != null)
            {
                movie.Genres = genres;
                
            }
            _context.Add(movie);
            return Save();
        }

        public bool DeleteMovie(MovieOnDvd movie)
        {
            _context.Remove(movie);
            return Save();
        }

        public bool DeleteMovies(ICollection<MovieOnDvd> movies)
        {
            _context.RemoveRange(movies);
            return Save();
        }

        public MovieOnDvd GetMovieById(int id)
        {

            return _context.MoviesOnDvd.FirstOrDefault(m => m.Id == id);
        }

        public MovieOnDvd GetMovieByName(string name)
        {
            return _context.MoviesOnDvd.FirstOrDefault(m => m.Name == name);
        }

        public decimal GetMovieRating(int movieId)
        {
            var reviews = _context.Reviews.Where(r => r.Movie.Id == movieId);
            if (reviews.Count() <= 0)
                return decimal.Zero;
            return Math.Round(reviews.Select(r => r.Rating).Average(), 2);
        }

        public ICollection<MovieOnDvd> GetMovies()
        {
            return _context.MoviesOnDvd.OrderBy(m => m.Id).ToList();
        }

        public ICollection<MovieOnDvd> GetMoviesByGenre(int genreId)
        {
            return _context.MoviesOnDvd.Where(m => m.Genres.Any(g => g.Id == genreId)).ToList();
        }

        public ICollection<MovieOnDvd> GetMoviesByIds(int[] ids)
        {
            var movies = new List<MovieOnDvd>();
            foreach (var id in ids)
            {
                movies.Add(GetMovieById(id));
            }
            return movies;
        }

        public ICollection<MovieOnDvd> GetMoviesByOwnerId(int ownerId)
        {
            return _context.MoviesOnDvd.Where(m => m.Owners.Any(o => o.Id == ownerId)).ToList();
        }

        public bool MovieExist(int movieId)
        {
            return _context.MoviesOnDvd.Any(m => m.Id == movieId);
        }

        public bool MovieExist(string movieName)
        {
            return _context.MoviesOnDvd.Any(m => m.Name.Trim().ToUpper() == movieName.Trim().ToUpper());
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateMovie(MovieOnDvd movie)
        {
            _context.Update(movie);
            return Save();
        }
    }
}
