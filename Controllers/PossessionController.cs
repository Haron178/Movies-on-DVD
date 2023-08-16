using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PossessionController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public PossessionController(IOwnerRepository ownerRepository, IMovieRepository movieRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _movieRepository = movieRepository;
        }
        [HttpPatch("getPossession")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult GetPossession([FromQuery] int[] movieIds, [FromQuery] int ownerId)
        {
            var movieIdsNotExists = movieIds.Any(id => !_movieRepository.MovieExist(id));
            if (!_ownerRepository.OwnerExist(ownerId))
            {
                ModelState.AddModelError("", "Owner not found");
                return StatusCode(404, ModelState);
            }
            if (movieIdsNotExists)
            {
                ModelState.AddModelError("", "Movie not found");
                return StatusCode(404, ModelState);
            }
            var movies = _movieRepository.GetMoviesByIds(movieIds);
            var owner = _ownerRepository.GetOwnerById(ownerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_ownerRepository.GetPossession(owner, movies))
            {
                ModelState.AddModelError("", "Something went wrong geting possession");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpPatch("dispossession")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult Dispossession([FromQuery] int[] movieIds, [FromQuery] int ownerId)
        {
            var movieIdsNotExistsInOwner = movieIds.Any(id => !_ownerRepository.GetOwnersByMovieId(id).Any(o => o.Id == ownerId));
            if (!_ownerRepository.OwnerExist(ownerId))
            {
                ModelState.AddModelError("", "Owner not found");
                return StatusCode(404, ModelState);
            }
            if (movieIdsNotExistsInOwner)
            {
                ModelState.AddModelError("", "Owner not own movie");
                return StatusCode(404, ModelState);
            }
            var movies = _movieRepository.GetMoviesByIds(movieIds);
            var owner = _ownerRepository.GetOwnerById(ownerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_ownerRepository.Dispossession(owner, movies))
            {
                ModelState.AddModelError("", "Something went wrong with dispossession");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
    }
}
