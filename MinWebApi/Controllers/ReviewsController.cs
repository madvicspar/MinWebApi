using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinWebApi.Models;

namespace MinWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// Constructor for the ReviewsController
        /// </summary>
        /// <param name="context">The db context</param>
        public ReviewsController(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns>A list of reviews</returns>
        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews
                .Include(x => x.Movie)
                .Include(y => y.Reviewer)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific review by ID
        /// </summary>
        /// <param name="id">The ID of the review to retrieve</param>
        /// <returns>The specified review</returns>
        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Movie)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(r => r.Id == id);

            return review == null ? NotFound() : review;
        }

        /// <summary>
        /// Adds a new review to the database
        /// </summary>
        /// <param name="review">The review to add</param>
        /// <returns>The created review</returns>
        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            review.Reviewer = await _context.Users.FindAsync(review.ReviewerId);
            review.Movie = await _context.Movies.FindAsync(review.MovieId);
            if (review.Movie == null)
            {
                return NotFound($"Movie with id = {review.MovieId} not found.");
            }
            if (review.Reviewer == null)
            {
                return NotFound($"User with id = {review.ReviewerId} not found.");
            }
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }
    }
}
