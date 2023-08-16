using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepository, IMovieRepository movieRepository, IMapper mapper)
        {
            _mapper = mapper;
            _genreRepository = genreRepository;
            _movieRepository = movieRepository;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult AddGenre([FromBody] GenreDto newGenre)
        {
            if (newGenre == null)
                return BadRequest(ModelState);
            if (_genreRepository.GenreExist(newGenre.Name))
            {
                ModelState.AddModelError("", "Genre already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var genreMap = _mapper.Map<Genre>(newGenre);
            if (!_genreRepository.AddGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GenreDto>))]
        public ActionResult GetGenres()
        {
            var genres = _mapper.Map<List<GenreDto>>(_genreRepository.GetGenres());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(genres);
        }

        [HttpGet("ByMovieId/{movieId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<GenreDto>))]
        [ProducesResponseType(400)]
        public ActionResult GetGenresByMovieId(int movieId)
        {
            if (!_movieRepository.MovieExist(movieId))
                return NotFound();
            var genres = _mapper.Map<List<GenreDto>>(_genreRepository.GetGenresByMovieId(movieId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(genres);
        }

        [HttpGet("{genreId}")]
        [ProducesResponseType(200, Type = typeof(GenreDto))]
        [ProducesResponseType(400)]
        public ActionResult GetGenreById(int genreId)
        {
            if (!_genreRepository.GenreExist(genreId))
                return NotFound();
            var genre = _mapper.Map<GenreDto>(_genreRepository.GetGenreById(genreId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(genre);
        }

        [HttpPut("{genreId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult UpdateGenre(int genreId, [FromBody] GenreDto updatedGenre)
        {
            if (!_genreRepository.GenreExist(genreId))
                return NotFound();
            if (updatedGenre == null || genreId != updatedGenre.Id)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_genreRepository.UpdateGenre(_mapper.Map<Genre>(updatedGenre)))
            {
                ModelState.AddModelError("", "Something went wrong updating genre");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        
        [HttpDelete("{genreId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult DeleteGenre(int genreId)
        {
            if (!_genreRepository.GenreExist(genreId))
                return NotFound();
            var genreToDelete = _genreRepository.GetGenreById(genreId);
            var movies = _movieRepository.GetMoviesByGenre(genreId).Where(m => m.Genres.Count() <= 1).ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_genreRepository.DeleteGenre(genreToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting genre");
                return StatusCode(500, ModelState);
            }
            if (movies != null)
            {
                if (!_movieRepository.DeleteMovies(movies))
                {
                    ModelState.AddModelError("", "Something went wrong deleting movies");
                    return StatusCode(500, ModelState);
                }
            }
            return Ok("Success");
        }
    }
}
