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

        public ReviewsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.Include(x => x.Movie).Include(y => y.Reviewer).ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.Include(r => r.Movie).Include(r => r.Reviewer)
                    .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            review.Reviewer = await _context.Users.FindAsync(review.ReviewerId);
            review.Movie = await _context.Movies.FindAsync(review.MovieId);
            if (review.Reviewer == null || review.Movie == null)
            {
                if (review.Movie == null)
                {
                    return NotFound($"Movie with id = {review.MovieId} not found.");
                }
                if (review.Reviewer == null)
                {
                    return NotFound($"User with id = {review.ReviewerId} not found.");
                }
            }
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }
    }
}
