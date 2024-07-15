using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinWebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MinWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntitiesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public EntitiesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.Include(x => x.Movie).Include(y => y.Reviewer).ToListAsync();
        }

        [HttpGet("movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(int id)
        {
            var applicationUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return applicationUser;
        }

        [HttpGet("reviews/{id}")]
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

        [HttpGet("movies/{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        [HttpPost("users")]
        public async Task<ActionResult<ApplicationUser>> PostUser(ApplicationUser applicationUser)
        {
            _context.Users.Add(applicationUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        [HttpPost("reviews")]
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

        [HttpPost("movies")]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }
    }
}
