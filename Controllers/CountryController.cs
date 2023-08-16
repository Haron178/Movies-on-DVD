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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IOwnerRepository ownerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
            _ownerRepository = ownerRepository;
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult AddGenre([FromBody] CountryDto newCountry)
        {
            if (newCountry == null)
                return BadRequest(ModelState);
            if (_countryRepository.CountryExist(newCountry.Name))
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var countryMap = _mapper.Map<Country>(newCountry);
            if (!_countryRepository.AddCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public ActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public ActionResult GetCountryById(int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryById(countryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();
            if (updatedCountry == null || countryId != updatedCountry.Id)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_countryRepository.UpdateCountry(_mapper.Map<Country>(updatedCountry)))
            {
                ModelState.AddModelError("","Something went wrong updating country");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();
            var country = _countryRepository.GetCountryById(countryId);
            var owners = _ownerRepository.GetOwnersByCountryId(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_countryRepository.DeleteCountry(country))
            {
                ModelState.AddModelError("", "Something went wrong deleting country");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
    }
}
