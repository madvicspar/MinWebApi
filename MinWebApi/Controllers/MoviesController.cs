using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinWebApi.Models;

namespace MinWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// Constructor for the MoviesController
        /// </summary>
        /// <param name="context">The db context</param>
        public MoviesController(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all movies
        /// </summary>
        /// <returns>A list of movies</returns>
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific movie by ID
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve</param>
        /// <returns>The specified movie</returns>
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            return movie == null ? NotFound() : movie;
        }

        /// <summary>
        /// Adds a new movie to the database
        /// </summary>
        /// <param name="movie">The movie to add</param>
        /// <returns>The created movie</returns>
        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }
    }
}
