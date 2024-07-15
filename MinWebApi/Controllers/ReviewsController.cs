using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinWebApi.Models;

namespace MinWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns>A list of reviews</returns>
        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    StatusCode(500, "Database access failed.");
                }

                return await dataBase.Reviews
                    .Include(x => x.Movie)
                    .Include(y => y.Reviewer)
                    .ToListAsync();
            }
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
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    return StatusCode(500, "Database access failed.");
                }

                var review = await dataBase.Reviews
                    .Include(r => r.Movie)
                    .Include(r => r.Reviewer)
                    .FirstOrDefaultAsync(r => r.Id == id);

                return review == null ? NotFound() : review;
            }
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
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    return StatusCode(500, "Database access failed.");
                }

                if (!dataBase.Movies.AnyAsync(m => m.Id == review.MovieId).Result)
                {
                    return NotFound($"Movie with id = {review.MovieId} not found.");
                }

                if (!dataBase.Users.AnyAsync(m => m.Id == review.ReviewerId).Result)
                {
                    return NotFound($"User with id = {review.ReviewerId} not found.");
                }

                dataBase.Reviews.Add(review);
                await dataBase.SaveChangesAsync();

                return CreatedAtAction("GetReview", new { id = review.Id }, review);
            }
        }
    }
}
