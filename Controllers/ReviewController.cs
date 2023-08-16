using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository,
            IMovieRepository movieRepository, IMapper mapper)
        {
            _mapper = mapper;
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _movieRepository = movieRepository;
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult AddReview([FromQuery] int movieId,[FromQuery] int reviewerId, [FromBody] ReviewDto newReview)
        {
            if (newReview == null)
                return BadRequest(ModelState);
            if (_reviewRepository.ReviewExist(newReview.Title))
            {
                ModelState.AddModelError("", "Review with this title already exists");
                return StatusCode(422, ModelState);
            }
            if (!_reviewerRepository.ReviewerExist(reviewerId))
            {
                ModelState.AddModelError("", "Reviewer not found");
                return StatusCode(404, ModelState);
            }
            if (!_movieRepository.MovieExist(movieId))
            {
                ModelState.AddModelError("", "Movie not found");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(newReview);
            if (!_reviewRepository.AddReview(movieId, reviewerId ,reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public ActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        public ActionResult GetReviewById(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReviewById(reviewId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpGet("ByReviewerId/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public ActionResult GetReviewsByReviewerId(int reviewerId)
        {
            var reviewMap = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsByReviewerId(reviewerId));
            if(reviewMap == null || reviewMap.Count() == 0)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewMap);
        }
        [HttpGet("ByMovieId/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public ActionResult GetReviewsByMovieId(int movieId)
        {
            var reviewMap = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsByMovieId(movieId));
            if (reviewMap == null || reviewMap.Count() == 0)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewMap);
        }
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound();
            if (updatedReview == null || reviewId != updatedReview.Id)
                return BadRequest(ModelState);
            if (!_reviewRepository.ReviewExist(reviewId))
            {
                ModelState.AddModelError("", "Review not found");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRepository.UpdateReview(_mapper.Map<Review>(updatedReview)))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound();
            var reviewToDelete = _reviewRepository.GetReviewById(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
    }
}
