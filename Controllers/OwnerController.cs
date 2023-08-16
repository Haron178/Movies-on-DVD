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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository,
            IMovieRepository movieRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _movieRepository = movieRepository;
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult AddOwner([FromQuery] int countryId, [FromBody] OwnerDto newOwner)
        {
            if (newOwner == null)
                return BadRequest(ModelState);
            if (_ownerRepository.OwnerExist(newOwner.Name))
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }
            if (!_countryRepository.CountryExist(countryId))
            {
                ModelState.AddModelError("", "Country not found");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ownerMap = _mapper.Map<Owner>(newOwner);
            if (!_ownerRepository.AddOwner(countryId, ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        public ActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        [ProducesResponseType(400)]
        public ActionResult GetOwnerById(int ownerId)
        {
            if (!_ownerRepository.OwnerExist(ownerId))
                return NotFound();
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwnerById(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("ByCountryId/{countryId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<OwnerDto>))]
        [ProducesResponseType(400)]
        public ActionResult GetOwnersByCountryId(int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnersByCountryId(countryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpGet("ByMovieId/{movieId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<OwnerDto>))]
        [ProducesResponseType(400)]
        public ActionResult GetOwnersByMovieId(int movieId)
        {
            if (!_movieRepository.MovieExist(movieId))
                return NotFound();
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnersByMovieId(movieId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (!_ownerRepository.OwnerExist(ownerId))
                return NotFound();
            if (updatedOwner == null || ownerId != updatedOwner.Id)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_ownerRepository.UpdateOwner(_mapper.Map<Owner> (updatedOwner)))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExist(ownerId))
                return NotFound();
            var ownerToDelete = _ownerRepository.GetOwnerById(ownerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
    }
}
