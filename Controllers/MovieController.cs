using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepository movieRepository, IGenreRepository genreRepository,
            IReviewRepository reviewRepository, IOwnerRepository ownerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _reviewRepository = reviewRepository;
            _genreRepository = genreRepository;
            _ownerRepository = ownerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieOnDvdDto>))]
        public ActionResult GetMovies()
        {
            var movies = _mapper.Map<List<MovieOnDvdDto>>(_movieRepository.GetMovies());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(movies);
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(200, Type = typeof(MovieOnDvdDto))]
        [ProducesResponseType(400)]
        public ActionResult GetMovie(int movieId)
        {
            if (!_movieRepository.MovieExist(movieId))
                return NotFound();
            var movie = _mapper.Map<MovieOnDvdDto>(_movieRepository.GetMovieById(movieId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(movie);
        }
        [HttpGet("ByGenreId/{genreId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<MovieOnDvdDto>))]
        [ProducesResponseType(400)]
        public ActionResult GetMoviesByGenreId(int genreId)
        {
            if (!_genreRepository.GenreExist(genreId))
                return NotFound();
            var movie = _mapper.Map<List<MovieOnDvdDto>>(_movieRepository.GetMoviesByGenre(genreId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (movie.Count <= 0)
                return NotFound();
            return Ok(movie);
        }
        [HttpGet("ByOwnerId/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<MovieOnDvdDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult GetMoviesByOwnerId(int ownerId)
        {
            if (!_ownerRepository.OwnerExist(ownerId))
            {
                ModelState.AddModelError("", "owner not found");
                return StatusCode(404, ModelState);
            }
            var movies = _mapper.Map<List<MovieOnDvdDto>>(_movieRepository.GetMoviesByOwnerId(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (movies.Count <= 0)
                return NotFound();
            return Ok(movies);
        }

        [HttpGet("{movieId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult GetMovieRating(int movieId)
        {
            if (!_movieRepository.MovieExist(movieId))
                return NotFound();
            var reviews = _reviewRepository.GetReviewsByMovieId(movieId);
            if (reviews.Count == 0)
            {
                ModelState.AddModelError("", "Movie has not reviews");
                return StatusCode(404, ModelState);
            }
            var rating = _movieRepository.GetMovieRating(movieId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult AddMovie([FromQuery] int[] genreIds, [FromBody] MovieOnDvdDto newMovie)
        {
            if (newMovie == null)
                return BadRequest(ModelState);
            if (_movieRepository.MovieExist(newMovie.Name))
            {
                ModelState.AddModelError("", "Movie already exists");
                return StatusCode(422, ModelState);
            }
            var genresNotExists = genreIds.Any(g => !_genreRepository.GenreExist(g));
            if (genresNotExists)
            {
                ModelState.AddModelError("", "Genre not found");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var movieMap = _mapper.Map<MovieOnDvd>(newMovie);
            if(!_movieRepository.CreateMovie(genreIds, movieMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");   
        }
        [HttpPut("{movieId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult UpdateMovie(int movieId, [FromBody] MovieOnDvdDto updatedMovie)
        {
            if (!_movieRepository.MovieExist(movieId))
                return NotFound();
            if (updatedMovie == null || movieId != updatedMovie.Id)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_movieRepository.UpdateMovie(_mapper.Map<MovieOnDvd>(updatedMovie)))
            {
                ModelState.AddModelError("", "Something went wrong updating movie");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpDelete("{movieId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepository.MovieExist(movieId))
                return NotFound();
            var movieToDelete = _movieRepository.GetMovieById(movieId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_movieRepository.DeleteMovie(movieToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting movie");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
    }
}
