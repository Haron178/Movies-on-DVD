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
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult AddReviewer([FromBody] ReviewerDto newReviewer)
        {
            if (newReviewer == null)
                return BadRequest(ModelState);
            if (_reviewerRepository.ReviewerExist(newReviewer.Name))
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewerMap = _mapper.Map<Reviewer>(newReviewer);
            if (!_reviewerRepository.AddReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public ActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewers);
        }
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        public ActionResult GetReviewerById(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();
            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewerById(reviewerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewer);
        }
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();
            if (updatedReviewer == null || reviewerId != updatedReviewer.Id)
                return BadRequest(ModelState);
            if (!_reviewerRepository.ReviewerExist(reviewerId))
            {
                ModelState.AddModelError("", "Reviewer not found");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewerRepository.UpdateReviewer(_mapper.Map<Reviewer>(updatedReviewer)))
            {
                ModelState.AddModelError("", "Something went wrong updating reviewer");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();
            var reviewerToDelete = _reviewerRepository.GetReviewerById(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
    }
}
